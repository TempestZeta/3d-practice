using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    // 애니메이션 컨트롤러 변경 관련
    [System.Serializable]
    public class AnimChanger
    {
        public RuntimeAnimatorController[] arrAnimator;

        public RuntimeAnimatorController ChanageAnimator(int index)
        {
            if (arrAnimator[index] != null)
                return arrAnimator[index];

            return null;
        }
    }
    // 여기에 애니메이션 컨트롤러를 담는다.
    public AnimChanger pAnimChange;

    // 지금 가지고 있는 무기 관련 배열
    bool[] haveWeapon = new bool[3];

    // 현재 들고 있는 무기
    int currWeapon;
    int nextWeapon = 0;

    // 무기 오브젝트 담음 + Renderer로 켰다 껐다
    public GameObject[] weaponObj;
    Dictionary<int, List<Renderer>> weaponRenderer = new Dictionary<int, List<Renderer>>();

    // 무기 제어 스크립트
    public Attack[] attacks = new Attack[4];

    BowAttack bowAtt;

    Animator anim;

    AudioSource audio;

    bool isMovable; // 이동이 가능한 상태일 때 true
    bool isRolling; // 구를 때 true
    bool isAttack;
    bool isSkill;
    bool isAlive;
    bool isDamaged;
    bool isDrink;
    bool normalMove;
    bool sideMove;
    bool canControl; // UI 열었을 때 입력 막기

    float verticalMove;
    float horizontalMove;

    Transform tr;

    // 공격 애니메이션 
    readonly int hashAttack = Animator.StringToHash("Attack1");
    readonly int hashSkill = Animator.StringToHash("Attack2");
    readonly int hashDamage = Animator.StringToHash("Damage");
    readonly int hashDeath = Animator.StringToHash("Death");
    // 블렌드 트리
    readonly int hashMove = Animator.StringToHash("isMove");
    readonly int hashFront = Animator.StringToHash("MoveFront");
    readonly int hashRight = Animator.StringToHash("MoveRight");
    readonly int hashSide = Animator.StringToHash("LeftRight");

    readonly int hashDrink = Animator.StringToHash("Drink");

    // 캐릭터 HP / 스태미나
    public Image hpBar;
    public float playerHP;
    public Image stmBar;
    public float playerStm;
    public Image bloodScreen;

    bool isDown;

    // 메인 카메라

    CameraControl mainCamera;

    // HP 바 색상
    readonly Color hpColor = new Vector4(0, 1.0f, 0.0f, 1.0f);
    Color currHPColor;
    Color currStmColor = new Vector4(1.0f, 1.0f, 0.0f, 1.0f);

    // 무기 교체 창
    public Image currWeaponUI;
    public Image nextWeaponUI;

    // 캡슐 콜라이더
    CapsuleCollider damageColl;

    // 넉백 연출
    Rigidbody playerRb;

    // 포션 갯수
    int havePortion;
    public ParticleSystem portionEffect;
    public AudioClip portionSound;

//-----------------------------------------------------------------------------------------

    private void Awake()
    {
        damageColl = GetComponent<CapsuleCollider>();
        anim = GetComponent<Animator>();
        tr = GetComponent<Transform>();
        attacks[0] = GetComponent<TwoHandSword>();
        attacks[1] = GetComponent<OneHandSword>();
        attacks[2] = GetComponent<Bow>();

        int check = 0;

        foreach (GameObject obj in weaponObj)
        {
            List<Renderer> tempList = new List<Renderer>();
            obj.GetComponentsInChildren<Renderer>(tempList);
            tempList.RemoveAt(0);
            weaponRenderer.Add(check, tempList);
            check++;
        }

        bowAtt = GetComponent<BowAttack>();

        bloodScreen.color = new Vector4(1.0f, 0.0f, 0.0f, 0.0f);

        mainCamera = GameObject.FindGameObjectWithTag("Camera").GetComponent<CameraControl>();

        playerRb = GetComponent<Rigidbody>();

        portionEffect.Stop();

        audio = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        isMovable = true;
        isRolling = false;
        isAttack = false;
        isSkill = false;
        isAlive = true;
        isDown = false;
        canControl = true;

        // 초기 HP 세팅 (세이브 로드 구현시 수정)
        playerHP = 100.0f;
        currHPColor = hpColor;
        hpBar.color = hpColor;

        // 초기 스태미나 세팅
        stmBar.color = currStmColor;

        // 초기 무기 세팅
        currWeapon = 0;

        for(int i = 0; i < haveWeapon.Length; i++)
        {
            haveWeapon[i] = true;
        }

        foreach (Renderer rend in weaponRenderer[currWeapon])
        {
            rend.enabled = true;
        }

        anim.runtimeAnimatorController = pAnimChange.ChanageAnimator(currWeapon);
    }

    private void OnEnable()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if(transform.position.y < -50.0f)
        {
            if (!isDown)
            {
                playerHP = 0.0f;
                anim.SetTrigger(hashDeath);
                UIMgr.uiMgr.PlayerDie();
                isDown = true;
            }

        }

        if (isAlive)
        {
            playerStm = GameManager.singleton.playerStm;
            havePortion = UIMgr.uiMgr.playerHavePortion;

            verticalMove = Input.GetAxis("Vertical");
            horizontalMove = Input.GetAxis("Horizontal");

            if(UIMgr.uiMgr.isOpenInven || UIMgr.uiMgr.isOpenControl)
            {
                canControl = false;
            }
            else
            {
                canControl = true;
            }

            if (canControl)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    isMovable = false;
                    isRolling = true;
                }
                else if (Input.GetMouseButtonDown(0))
                {
                    isMovable = false;
                    isAttack = true;
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    isMovable = false;
                    isSkill = true;
                }
                else if (Input.GetKeyDown(KeyCode.Tab))
                {
                    WeaponChange(nextWeapon);
                    currWeapon = nextWeapon;
                }
                else if (Input.GetKeyDown(KeyCode.T))
                {
                    if(havePortion > 0)
                    {
                        isMovable = false;
                        isDrink = true;
                    }

                }
                else if (Input.GetKey(KeyCode.Q))
                {
                    horizontalMove = -1.0f;
                    sideMove = true;
                }
                else if (Input.GetKey(KeyCode.E))
                {
                    horizontalMove = 1.0f;
                    sideMove = true;

                }
                else if (Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.E))
                {
                    sideMove = false;
                    horizontalMove = 0.0f;
                }
            }

            if (isMovable)
            {
                if (sideMove)
                {
                    anim.SetBool(hashSide, true);
                }
                else
                {
                    anim.SetBool(hashMove, true);
                    tr.Rotate(Vector3.up, horizontalMove * 3.0f);
                }
                
                anim.SetFloat(hashFront, verticalMove);
                anim.SetFloat(hashRight, horizontalMove);

                if (verticalMove == 0.0f && horizontalMove == 0.0f)
                {
                    anim.SetBool(hashMove, false);
                    anim.SetBool(hashSide, false);
                    sideMove = false;
                }
            }
            else if (isRolling)
            {
                if (GameManager.singleton.playerStm > 25.0f)
                {
                    anim.SetTrigger("Rolling");
                    anim.SetFloat(hashFront, verticalMove);
                    anim.SetFloat(hashRight, horizontalMove);
                    GameManager.singleton.playerStm -= 10.0f;
                    StartCoroutine(Rolling());
                }
                isRolling = false;
                isMovable = true;
            }
            else if (isAttack)
            {
                if(currWeapon != 2)
                {
                    bowAtt.isUseArrow = false;

                    if (attacks[currWeapon].AttackAction())
                    {
                        anim.SetTrigger(hashAttack);
                    }
                    isMovable = true;
                    isAttack = false;
                }
                else
                {
                    bowAtt.AimArrow();
                    isMovable = true;
                    isAttack = false;
                }

            }
            else if (isSkill)
            {
                if(currWeapon != 2)
                {
                    bowAtt.isUseArrow = false;

                    if (attacks[currWeapon].SkillAction())
                    {
                        anim.SetTrigger(hashSkill);
                    }
                    isMovable = true;
                    isSkill = false;
                }
                else
                {
                    bowAtt.EndAim();
                    isMovable = true;
                    isSkill = false;
                }
            }
            else if (isDrink)
            {
                anim.SetTrigger(hashDrink);
                isMovable = true;
                isDrink = false;
            }

            if (GameManager.singleton.playerStm <= 100.0f)
            {
                GameManager.singleton.playerStm += 0.2f;
            }

            DisplayHPbar();

            if (playerHP <= 0.0f)
            {
                isAlive = false;
            }

            NextWeapon();
            //currWeaponUI.sprite = GameManager.singleton.weaponImage[currWeapon];
            //currWeaponUI.color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);

        }
    }

    public void GetItem(int index)
    {
        if (index == 3 || index == 4) return;

        haveWeapon[index] = true;
    }


    void NextWeapon()
    {

        int check = (currWeapon + 1) % 3;

        nextWeapon = check;
        //nextWeaponUI.sprite = GameManager.singleton.weaponImage[nextWeapon];
        //nextWeaponUI.color = new Vector4(1.0f, 1.0f, 1.0f, 0.5f);

        //int check = 0;
        //
        //for (int i = 1; i < 3; i++)
        //{
        //    check = (currWeapon + i) % 3;
        //
        //    if (haveWeapon[check])
        //    {
        //        nextWeapon = check;
        //        nextWeaponUI.sprite = GameManager.singleton.weaponImage[nextWeapon];
        //        nextWeaponUI.color = new Vector4(1.0f, 1.0f, 1.0f, 0.5f);
        //        break;
        //    }   
        //}

    }

    void WeaponChange(int index)
    {
        if (index == currWeapon)
            return;

        if (haveWeapon[index])
        {
            foreach (Renderer rend in weaponRenderer[currWeapon])
            {
                rend.enabled = false;
            }
            // 한손검일 때 한 쌍 오브젝트 비활성화
            if (currWeapon == 1)
            {
                foreach (Renderer rend in weaponRenderer[4])
                {
                    rend.enabled = false;
                }
            }

            foreach (Renderer rend in weaponRenderer[index])
            {
                rend.enabled = true;
            }
            // 한손검일 때 한 쌍 오브젝트 활성화
            if (index == 1)
            {
                foreach (Renderer rend in weaponRenderer[4])
                {
                    rend.enabled = true;
                }
            }

            anim.runtimeAnimatorController = pAnimChange.ChanageAnimator(index);

            currWeapon = index;
        }
    }

    void DisplayHPbar()
    {
        if((playerHP / 100) > 0.5f)
        {
            currHPColor.r = (1 - (playerHP / 100.0f)) * 2.0f;
        }
        else
        {
            currHPColor.g = (playerHP / 100.0f) * 2.0f;
        }

        hpBar.color = currHPColor;
        hpBar.fillAmount = (playerHP / 100.0f);
        stmBar.fillAmount = (playerStm / 100.0f);
    }
    
    public void PlayerDamage(float damage)
    {
        if (isDamaged) return;
        if (!isAlive) return;

        playerHP -= damage;

        StartCoroutine(ShowBloodScreen());
        mainCamera.SendMessage("CameraDamage");

        if(playerHP > 0.0f)
        {
            anim.SetTrigger(hashDamage);
        }
        else
        {
            anim.SetTrigger(hashDeath);
            UIMgr.uiMgr.PlayerDie();
        }
    }

    IEnumerator Rolling()
    {
        isDamaged = true;

        yield return new WaitForSeconds(0.5f);

        isDamaged = false;
    }

    IEnumerator ShowBloodScreen()
    {
        bloodScreen.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);

        while (true)
        {
            float alpha = bloodScreen.color.a;

            if(alpha > 0.0f)
            {
                alpha -= Time.deltaTime * 2.0f;
                bloodScreen.color = new Color(1.0f, 0.0f, 0.0f, alpha);
                yield return new WaitForSeconds(0.2f);
            }
            else
            {
                yield break;
            }
        }
    }

    

    void KnockBack(Vector3 enemyPos)
    {
        Vector3 dist = transform.position - enemyPos;

        playerRb.AddForce(dist * 500.0f, ForceMode.Impulse);
    }
    
    void DrinkPortion()
    {
        playerHP += 50.0f;
        UIMgr.uiMgr.playerHavePortion--;
        UIMgr.uiMgr.RemoveItem(3);
        StartCoroutine(PortionEffect());
    }

    IEnumerator PortionEffect()
    {
        audio.PlayOneShot(portionSound, PlayerPrefs.GetFloat("Volume"));
        portionEffect.Play();
        yield return new WaitForSeconds(1.0f);
        portionEffect.Stop();
    }

}

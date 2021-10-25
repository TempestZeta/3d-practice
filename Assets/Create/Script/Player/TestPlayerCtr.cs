using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerCtr : MonoBehaviour
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
    bool[] haveWeapon = new bool[4];
   
    // 현재 들고 있는 무기
    int currWeapon;
    
    // 무기 오브젝트 담음 + Renderer로 켰다 껐다
    public GameObject[] weaponObj;
    Dictionary<int, List<Renderer>> weaponRenderer = new Dictionary<int, List<Renderer>>();
   
    // 무기 제어 스크립트
    public Attack[] attacks = new Attack[4];

    Animator anim;

    bool isMovable; // 이동이 가능한 상태일 때 true
    bool isRolling; // 구를 때 true
    bool isAttack;
    bool isSkill;

    float verticalMove;
    float horizontalMove;

    Transform tr;

    // 공격 애니메이션 
    readonly int hashAttack = Animator.StringToHash("Attack1");
    readonly int hashSkill = Animator.StringToHash("Attack2");

    // 블렌드 트리
    readonly int hashMove  = Animator.StringToHash("isMove");
    readonly int hashFront = Animator.StringToHash("MoveFront");
    readonly int hashRight = Animator.StringToHash("MoveRight");

    private void Awake()
    {
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
    }

    // Start is called before the first frame update
    void Start()
    {
        isMovable = true;
        isRolling = false;
        isAttack = false;
        isSkill = false;

        // 초기 무기 세팅
        currWeapon = 0;

        haveWeapon[currWeapon] = true;
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
        verticalMove = Input.GetAxis("Vertical");
        horizontalMove = Input.GetAxis("Horizontal");

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

        if (isMovable)
        {
            anim.SetBool(hashMove, true);
            anim.SetFloat(hashFront, verticalMove);
            anim.SetFloat(hashRight, horizontalMove);
            tr.Rotate(Vector3.up, horizontalMove);

            if (verticalMove == 0.0f && horizontalMove == 0.0f)
            {
                anim.SetBool(hashMove, false);
            }
        }
        else if (isRolling)
        {
            anim.SetTrigger("Rolling");
            anim.SetFloat(hashFront, verticalMove);
            anim.SetFloat(hashRight, horizontalMove);
            isRolling = false;
            isMovable = true;
        }
        else if (isAttack)
        {
            anim.SetTrigger(hashAttack);
            isMovable = true;
            isAttack = false;
        }
        else if (isSkill)
        {
            anim.SetTrigger(hashSkill);
            isMovable = true;
            isSkill = false;
        }
    }

    public void GetItem(int index)
    {
        haveWeapon[index] = true;
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

}

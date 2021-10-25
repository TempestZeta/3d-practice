using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowAttack : MonoBehaviour
{
    public Transform firePos;
    
    public GameObject arrow;
    public Camera mainCamera;
    public Camera aimCamera;
    public bool isUseArrow;

    public Transform aimPoint;

    int arrowSize;

    bool isFire;
    WaitForSeconds waitFire = new WaitForSeconds(2.0f);

    Animator anim;

    //List<GameObject> arrowList = new List<GameObject>();
    List<ArrowCheck> arrowList = new List<ArrowCheck>();

    readonly int hashAim = Animator.StringToHash("Aiming");
    readonly int hashFire = Animator.StringToHash("Fire");

    // 소리
    public AudioClip fireSound;
    AudioSource soundPlayer;

    public enum BOW_STATE
    {
        NONE = 0,
        AIM
    }

    public BOW_STATE state;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        soundPlayer = GetComponent<AudioSource>();

    }

    // Start is called before the first frame update
    void Start()
    {
        arrowSize = 20;

        for(int i = 0; i < arrowSize; i++)
        {
            GameObject tempArrow = Instantiate(arrow);
            tempArrow.SetActive(false);
            arrowList.Add(tempArrow.GetComponent<ArrowCheck>());
        }
        isUseArrow = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isUseArrow) return;

        if(state == BOW_STATE.AIM)
        {
            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");

            transform.Rotate(Vector3.up, x);
            transform.Rotate(Vector3.right, -y);

            UIMgr.uiMgr.ShowAimPoint(aimCamera.WorldToScreenPoint(aimPoint.position));

            if (Input.GetMouseButtonDown(0))
            {
                if (!isFire)
                {
                    FireArrow();
                    anim.SetTrigger(hashFire);
                }
                
            }
            else if (Input.GetMouseButtonDown(1))
            {
                EndAim();
            }
        }
    }

    public void AimArrow()
    {
        isUseArrow = true;
        anim.SetBool(hashAim, true);
        mainCamera.SendMessage("CameraTranslate", aimCamera);
        state = BOW_STATE.AIM;
    }

    void FireArrow()
    {
        foreach(ArrowCheck arrow in arrowList)
        {
            if (!arrow.isFire)
            {
                GameManager.singleton.playerStm -= 10.0f;
                isFire = true;
                GameObject obj = arrow.gameObject;
                obj.SetActive(true);
                arrow.ArrowFire(firePos);
                StartCoroutine(WaitFire());
                soundPlayer.PlayOneShot(fireSound, PlayerPrefs.GetFloat("Volume"));
                break;
            }
        }
    }

    public void EndAim()
    {
        mainCamera.SendMessage("ResetCamera", aimCamera);
        state = BOW_STATE.NONE;
        anim.SetBool(hashAim, false);
        isUseArrow = false;
        transform.localRotation = Quaternion.Euler(0.0f, transform.eulerAngles.y, 0.0f);
        UIMgr.uiMgr.EndAimPoint();
    }

    IEnumerator WaitFire()
    {   
        yield return waitFire;

        isFire = false;
    }
}

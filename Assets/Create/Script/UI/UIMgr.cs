using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIMgr : MonoBehaviour
{
    public Canvas onGameUI;
    public Canvas invectoryUI;

    public GameObject interactionUI;
    public Text interactionText;

    public Text message;

    public bool isOpenInven;
    public bool isOpenControl;
    public bool isInteraction;

    public Image youDie;
    float youDieAlpha = 0.0f;

    public int playerHavePortion;
    public int playerHaveArrow;

    public WeaponUI weaponUI;

    static public UIMgr uiMgr;

    public AudioClip dieSound;
    AudioSource audio;

    // 에임포인트
    public Image aimPoint;

    // Start is called before the first frame update
    void Start()
    {
        playerHavePortion = 0;
        uiMgr = this;
        interactionUI.SetActive(false);
        isOpenInven = false;
        CloseInteraction();
        message.text = null;
        audio = GetComponent<AudioSource>();
        aimPoint.enabled = false;

        weaponUI = GetComponent<WeaponUI>();

        ShowMessage("해골 너머 봉인의 서를 찾아라.");

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.I))
        {
            isOpenInven = !isOpenInven;
            invectoryUI.GetComponent<Inventory>().ResetDisc();
        }

        if (isOpenInven)
        {
            onGameUI.enabled = false;
            invectoryUI.enabled = true;
        }
        else
        {
            onGameUI.enabled = true;
            invectoryUI.enabled = false;
        }
    }

    void GetItem(int index)
    {
        invectoryUI.GetComponent<Inventory>().InsertSlot(index);
    }

    public void RemoveItem(int index)
    {
        invectoryUI.GetComponent<Inventory>().RemoveItem(index);
    }

    public void OpenInteraction(string text)
    {
        interactionUI.SetActive(true);
        isInteraction = true;
        interactionText.text = text;
    }

    public void CloseInteraction()
    {
        interactionUI.SetActive(false);
        interactionText.text = null;
        isInteraction = false;
    }

    public void PlayerDie()
    {
        StartCoroutine(ViewYouDie());
    }

    public IEnumerator ViewYouDie()
    {
        audio.PlayOneShot(dieSound, PlayerPrefs.GetFloat("Volume"));

        while (true)
        {
            if(youDieAlpha < 1.0f)
            {
                youDieAlpha += Time.deltaTime;
                youDie.color = new Vector4(1.0f, 1.0f, 1.0f, youDieAlpha);
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                SceneManager.LoadScene(1);
                yield break;
            }
        }
    }

    public void ShowMessage(string text)
    {
        StartCoroutine(MessageAppear(text));
    }

    public IEnumerator MessageAppear(string text)
    {
        message.text = text;
        yield return new WaitForSeconds(2.0f);
        message.text = null;
    }

    public void ShowAimPoint(Vector3 pos)
    {
        aimPoint.enabled = true;
        aimPoint.transform.position = new Vector3(pos.x -10.0f, pos.y + 25.0f, 0.0f);
        //aimPoint.transform.Translate(Vector3.forward * 10.0f);
    }

    public void EndAimPoint()
    {
        aimPoint.enabled = false;
    }

}

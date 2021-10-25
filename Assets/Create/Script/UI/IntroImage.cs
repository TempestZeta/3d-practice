using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroImage : MonoBehaviour
{
    public Image black;
    public Image introImage;
    public AudioSource bgMusic;

    GameObject selectPanel;
    GameObject message;

    float timeCheck = 1.0f;
    float fadeCheck = 1.0f;

    bool isStart;
    bool userSelect;

    // Start is called before the first frame update
    void Start()
    {
        isStart = true;
        selectPanel = GameObject.Find("Select");
        selectPanel.SetActive(false);
        message = GameObject.Find("Message");
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            if(timeCheck > 0.0f)
            {
                black.color = new Vector4(0.0f, 0.0f, 0.0f, timeCheck);
                timeCheck -= Time.deltaTime;
            }
        }

        if (Input.anyKey)
        {

            selectPanel.SetActive(true);
            message.SetActive(false);
            //if(timeCheck > 0.0f)
            //{
            //    black.color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
            //    timeCheck = 0.0f;
            //}
            //
            //StartCoroutine(FadeImage());
        }
    }



}

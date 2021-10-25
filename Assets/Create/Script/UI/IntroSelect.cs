using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroSelect : MonoBehaviour
{
    public Image backImg;

    float timeCheck;

    RectTransform title;

    // Start is called before the first frame update
    void Start()
    {
        title = GameObject.Find("Title").GetComponent<RectTransform>();

        title.Translate(Vector3.up * 10.0f);

        timeCheck = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameStart()
    {
        PlayerPrefs.SetInt("Difficult", 1);
        PlayerPrefs.SetFloat("Volume", 1.0f);
        StartCoroutine(FadeImage(1));
    }

    public void GoConfig()
    {
        StartCoroutine(FadeImage(3));
    }

    public void GameExit()
    {
        Application.Quit();
    }

    IEnumerator FadeImage(int sceneIdx)
    {
        while (true)
        {
            if (timeCheck > 0.0f)
            {
                timeCheck -= Time.deltaTime * 2.0f;
                backImg.color = new Vector4(1.0f, 1.0f, 1.0f, timeCheck);
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                SceneManager.LoadScene(sceneIdx);
                yield break;
            }
        }
    }

}

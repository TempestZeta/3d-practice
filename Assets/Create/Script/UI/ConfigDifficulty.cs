using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigDifficulty : MonoBehaviour
{
    public Image[] check;

    Vector4 seeColor;
    Vector4 noSeeColor;

    // Start is called before the first frame update
    void Start()
    {
        //seeColor = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
        //noSeeColor = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectDifficult(int index)
    {
        for(int i = 0; i < 3; i++)
        {
            if (i == index)
                check[i].color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);//seeColor;
            else
                check[i].color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);//noSeeColor;
        }

        PlayerPrefs.SetInt("Difficult", index);
    }

}

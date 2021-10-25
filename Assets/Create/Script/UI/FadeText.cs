using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FadeText : MonoBehaviour
{
    TextMeshProUGUI text;
    

    float check = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text.fontMaterial.SetFloat("_OutlineSoftness", check);

        if(check < 1.0f)
        {
            check += Time.deltaTime / 2.0f;
        }
        else if(check >= 1.0f)
        {
            Application.Quit();
        }

    }
}

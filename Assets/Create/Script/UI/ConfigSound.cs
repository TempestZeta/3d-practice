using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigSound : MonoBehaviour
{
    public Image speaker;
    public Sprite[] speakerImg;
    public Slider volume;

    // Start is called before the first frame update
    void Start()
    {
        volume = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(volume.value == 0.0f)
        {
            speaker.sprite = speakerImg[1];
        }
        else
        {
            speaker.sprite = speakerImg[0];
        }
    }

    public void SetVolume()
    {
        PlayerPrefs.SetFloat("Volume", volume.value);
    }

}

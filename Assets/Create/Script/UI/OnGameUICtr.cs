using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnGameUICtr : MonoBehaviour
{
    public GameObject hpUI;
    public GameObject weaponUI;
    public GameObject controlUI;
    bool isControlOpen;

    // Start is called before the first frame update
    void Start()
    {
        hpUI.SetActive(true);
        weaponUI.SetActive(true);
        controlUI.SetActive(false);
        isControlOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenControl()
    {
        isControlOpen = !isControlOpen;
        controlUI.SetActive(isControlOpen);
    }
}

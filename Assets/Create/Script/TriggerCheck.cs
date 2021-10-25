using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCheck : MonoBehaviour
{
    bool isPlayerIn = false;

    public bool isTrue
    {
        get { return isPlayerIn; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
            isPlayerIn = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isPlayerIn = false;
            UIMgr.uiMgr.CloseInteraction();
        }
            

        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

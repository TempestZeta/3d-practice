using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherCheck : MonoBehaviour
{

    public TriggerCheck tc;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (tc.isTrue)
        {
            UIMgr.uiMgr.OpenInteraction("상자 열기");

            if (Input.GetKeyDown(KeyCode.F))
            {
                GameManager.singleton.archerCheck++;
                UIMgr.uiMgr.CloseInteraction();
                Destroy(gameObject);
            }
        }
    }
}

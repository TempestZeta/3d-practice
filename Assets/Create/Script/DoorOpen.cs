using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    bool isOpen = false;
    public GameObject rightDoor;
    public GameObject leftDoor;
    public GameObject doorSwitch;
    string openTheDoor;
    int check = 0;

    private void Awake()
    {
        //rightDoor = GameObject.Find("RDoor");
        //leftDoor = GameObject.Find("LDoor");
        openTheDoor = "문 열기";
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (doorSwitch.GetComponent<TriggerCheck>().isTrue)
        {
            if(!isOpen)
                UIMgr.uiMgr.OpenInteraction(openTheDoor);

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (!isOpen)
                {
                    isOpen = true;
                    UIMgr.uiMgr.CloseInteraction();
                }
                    
            }
        }

        if (isOpen)
        {
            if(check <= 100)
            {
                rightDoor.transform.Rotate(rightDoor.transform.up, 1.0f, Space.Self);
                leftDoor.transform.Rotate(leftDoor.transform.up, -1.0f, Space.Self);
                check++;
            }
        }
    }
}

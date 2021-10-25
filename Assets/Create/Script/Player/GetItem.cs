using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItem : MonoBehaviour
{
    public int itemName;
    GameObject player;
    PlayerControl pc;

    // 스위치 + 주인공 체크
    public GameObject itemSwitch;
    TriggerCheck tc;
    string itemText;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        pc = player.GetComponent<PlayerControl>();
        tc = itemSwitch.GetComponent<TriggerCheck>();
    }

    // Start is called before the first frame update
    void Start()
    {
        itemText = "획득 " + GameManager.singleton.itemInfomation[itemName]["name"];
    }

    // Update is called once per frame
    void Update()
    {
        if (tc.isTrue)
        {
            UIMgr.uiMgr.OpenInteraction(itemText);

            if (Input.GetKeyDown(KeyCode.F))
            {
                pc.SendMessage("GetItem", itemName);
                UIMgr.uiMgr.SendMessage("GetItem", itemName);
                UIMgr.uiMgr.CloseInteraction();
                Destroy(gameObject);
            }
        }
    }
}

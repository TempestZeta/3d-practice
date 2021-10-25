using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public Button[] invenBtn;
    bool[] isSlot;

    public Text title;
    public Text itemDisc;

    // Start is called before the first frame update
    void Start()
    {   
        isSlot = new bool[12];
        InsertSlot(0);
        InsertSlot(1);
        InsertSlot(2);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void InsertSlot(int index)
    {
        int check = 0;

        foreach(Button btn in invenBtn)
        {
            if(isSlot[check] == false)
            {
                btn.GetComponent<Image>().sprite = GameManager.singleton.weaponImage[index];
                btn.GetComponent<ItemButton>().itemIndex = index;
                isSlot[check] = true;

                if(index == 3)
                {
                    UIMgr.uiMgr.playerHavePortion++;
                }
                else if(index == 4)
                {
                    UIMgr.uiMgr.playerHaveArrow++;
                }
                break;
            }
            check++;
        }
    }

    public void RemoveItem(int index)
    {
        int check = 0;
        foreach(Button btn in invenBtn)
        {

            if(btn.GetComponent<ItemButton>().itemIndex == index)
            {
                btn.GetComponent<Image>().sprite = null;
                btn.GetComponent<ItemButton>().itemIndex = -1;
                isSlot[check] = false;
                break;
            }
            check++;
        }
    }

    public void ResetDisc()
    {
        title.text = " ";
        itemDisc.text = " ";
    }

}

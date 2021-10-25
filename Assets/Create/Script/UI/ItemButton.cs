using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public int itemIndex;
    public Text title;
    public Text itemDisc;

    Image img;
    // Start is called before the first frame update
    void Awake()
    {
        itemIndex = -1;
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(img.sprite == null)
        {
            img.color = new Vector4(1.0f, 1.0f, 1.0f, 0.0f);
        }
        else
        {
            img.color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
        }
    }

    public void ButtonClickEvent()
    {
        if (itemIndex == -1) return;

        title.text = GameManager.singleton.itemInfomation[itemIndex]["name"].ToString();
        itemDisc.text = GameManager.singleton.itemInfomation[itemIndex]["itemDis"].ToString();

    }



}

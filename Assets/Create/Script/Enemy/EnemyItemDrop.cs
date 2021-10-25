using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyItemDrop : MonoBehaviour
{
    public GameObject[] itemList;

    int whatItem;

    // Start is called before the first frame update
    void Start()
    {
        whatItem = Random.Range(0, itemList.Length + 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DropItem()
    {
        if(whatItem < itemList.Length)
        {
            GameObject item = Instantiate(itemList[whatItem], transform);
            item.transform.SetParent(null);
        }
    }

}

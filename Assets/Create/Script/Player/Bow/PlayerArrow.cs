using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArrow : MonoBehaviour
{
    public GameObject arrowPf;
    public bool isFire;

    public PlayerArrow(GameObject pf)
    {
        arrowPf = pf;
        arrowPf.SetActive(false);
        isFire = false;
    }

    public void Fire(Transform pos)
    {
        arrowPf.SetActive(true);
        isFire = true;
        arrowPf.transform.position = pos.position;
        arrowPf.transform.rotation = pos.rotation;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {

        }
        else if(other.tag == "Obstacle")
        {

        }
    }

}

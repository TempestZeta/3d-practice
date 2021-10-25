using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCheck : MonoBehaviour
{
    public Attack ohs;

    

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            ohs.Damage(other);
        }
    }

    private void Awake()
    {


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

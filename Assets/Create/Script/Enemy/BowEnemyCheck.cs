using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowEnemyCheck : MonoBehaviour
{
    public bool isFire;
    public float arrowSpeed;
    WaitForSeconds ws = new WaitForSeconds(4.0f);
    Transform playerTr;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.SendMessage("PlayerDamage", 5.0f);
            Destroy(gameObject);
        }
        else if(other.tag == "Obsatacle" || other.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerTr = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isFire)
        {
            transform.Translate(Vector3.forward * arrowSpeed * Time.deltaTime, Space.Self);
        }
    }

    IEnumerator ArrowDistroy()
    {
        yield return ws;

        Destroy(gameObject);
    }

    public void FireArrow()
    {   
        transform.LookAt(playerTr);
        isFire = true;
        StartCoroutine(ArrowDistroy());
    }

}

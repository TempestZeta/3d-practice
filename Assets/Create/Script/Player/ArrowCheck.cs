using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCheck : MonoBehaviour
{
    public bool isFire;
    public float arrowDamage = 5.0f;
    public float arrowSpeed = 100.0f;

    WaitForSeconds ws = new WaitForSeconds(4.0f);

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.SendMessage("EnemyDamage", 5.0f);
            isFire = false;
            gameObject.SetActive(false);
        }
        else if(other.tag == "Obstacle" || other.tag == "Wall")
        {
            isFire = false;
            gameObject.SetActive(false);
        }
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isFire)
        {
            transform.Translate(Vector3.forward * arrowSpeed * Time.deltaTime);
        }
    }

    IEnumerator ArrowDestroy()
    {
        yield return ws;

        isFire = false;
        gameObject.SetActive(false);
    }

    public void ArrowFire(Transform tr)
    {
        isFire = true;
        transform.position = tr.position;
        transform.rotation = tr.rotation;
        StartCoroutine(ArrowDestroy());
    }

}

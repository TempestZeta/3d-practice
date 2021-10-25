using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMove : MonoBehaviour
{
    public bool isMove;
    public bool isRun;
    public bool isWalk;

    public float moveSpeed;
    public float runSpeed;
    float speed;

    Transform bossTr;
    Transform playerTr;

    // Start is called before the first frame update
    void Start()
    {
        bossTr = GetComponent<Transform>();
        playerTr = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        speed = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMove)
        {
            bossTr.LookAt(playerTr);

            if (isWalk)
            {
                if(speed > moveSpeed)
                {
                    speed += Time.deltaTime * 2.0f;
                }

                bossTr.Translate(Vector3.forward * speed);
            }
            else if (isRun)
            {
                if(speed > runSpeed)
                {
                    speed += Time.deltaTime * 2.0f;
                }

                bossTr.Translate(Vector3.forward * runSpeed);
            }
        }
        else
        {
            speed = 0.0f;
        }
    }

    public void LookPlayer()
    {
        bossTr.LookAt(playerTr);
    }

}

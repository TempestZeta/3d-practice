using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NormalEnemyMove : EnemyMove
{
    public GameObject sirusi;

    bool isMove;
    float moveTime;
    
    // Start is called before the first frame update
    void Start()
    {
        enemyNav = GetComponent<NavMeshAgent>();
        enemyNav.autoBraking = false;
        EnemyPatroll();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPatrol) return;

        if(moveTime > 5.0f)
        {
            nextPosition = new Vector3((transform.position.x + Random.Range(-10.0f, 10.0f)), transform.position.y, (transform.position.z + Random.Range(-10.0f, 10.0f)));
            //Instantiate(sirusi, nextPosition, Quaternion.identity);
            StartCoroutine(WaitPatrol(Random.Range(1.0f, 5.0f)));
        }

        if(isMove)
            moveTime += Time.deltaTime;

        //if (enemyNav.velocity.sqrMagnitude >= 0.2f * 0.2f &&
        //   enemyNav.remainingDistance <= 0.5f)
        //{
        //    float waitTime = Random.Range(1.0f, 5.0f);
        //    StartCoroutine(WaitPatrol(waitTime));
        //    //EnemyPatroll();
        //}
    }

    public override void EnemyPatroll()
    {
        if (enemyNav.isPathStale) return;

        isMove = true;
        nextPosition = new Vector3((transform.position.x + Random.Range(-10.0f, 10.0f)), transform.position.y, (transform.position.z + Random.Range(-10.0f, 10.0f)));
        enemyNav.destination = nextPosition;
        enemyNav.speed = moveSpeed;
    }

    public override void TraceTarget(Vector3 player)
    {
        if (enemyNav.isPathStale) return;
        
        enemyNav.destination = player;
        enemyNav.speed = traceSpeed;
        enemyNav.isStopped = false;
    }

    public override void Stop()
    {
        enemyNav.isStopped = true;
        enemyNav.velocity = Vector3.zero;
        isPatrol = false;
    }

    IEnumerator WaitPatrol(float time)
    {
        enemyNav.destination = nextPosition;
        enemyNav.isStopped = true;
        isMove = false;
        moveTime = 0.0f;

        yield return new WaitForSeconds(time);

        isMove = true;
        enemyNav.isStopped = false;
        enemyNav.speed = moveSpeed;
    }

}

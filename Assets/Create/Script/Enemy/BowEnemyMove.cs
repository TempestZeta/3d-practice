using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BowEnemyMove : EnemyMove
{
    Transform enemyTr;
    Transform playerTr;

    private void Awake()
    {
        enemyNav = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyTr = GetComponent<Transform>();
        playerTr = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemyNav.autoBraking = false;
        EnemyPatroll();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPatrol) return;

        enemySpeed = enemyNav.velocity.magnitude;

        if (enemyNav.velocity.sqrMagnitude >= 0.2f * 0.2f &&
           enemyNav.remainingDistance <= 0.5f)
        {
            float waitTime = Random.Range(5.0f, 10.0f);
            StartCoroutine(WaitPatrol(waitTime));
        }
    }

    public override void EnemyPatroll()
    {
        if (enemyNav.isPathStale) return;

        nextPosition = new Vector3(Random.Range(-3.0f, 3.0f), 0.0f, Random.Range(-3.0f, 3.0f));
        enemyNav.destination = nextPosition;
        enemyNav.speed = moveSpeed;
    }

    public override void TraceTarget(Vector3 player)
    {
        isPatrol = false;
        enemyTr.LookAt(playerTr);
    }

    public override void Stop()
    {
        enemyNav.isStopped = true;
        enemyNav.velocity = Vector3.zero;
        isPatrol = false;
    }

    IEnumerator WaitPatrol(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        EnemyPatroll();
    }

}

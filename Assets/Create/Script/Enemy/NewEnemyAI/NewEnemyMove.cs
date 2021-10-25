using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewEnemyMove : MonoBehaviour
{

    NewEnemyAI enemyAI;
    Transform enemyTr;

    public float moveSpeed;
    public float traceSpeed;
    public float enemySpeed;

    // 이동 범위 
    public float moveRange;
    // 다음 위치
    Vector3 nextPosition;

    // 이동 여부
    public bool isPatrol;
    public bool isTrace;

    // NavMesh 관련
    NavMeshAgent enemyNav;

    WaitForSeconds impossiblePath = new WaitForSeconds(5.0f);

    private void Awake()
    {
        enemyAI = GetComponent<NewEnemyAI>();
        enemyTr = GetComponent<Transform>();
        enemyNav = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetNextPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPatrol)
        {
            enemySpeed = enemyNav.speed;

            enemyNav.isStopped = false;

            float dist = Vector3.Distance(enemyTr.position, nextPosition);

            if (dist < 0.5f)
            {

                SetNextPosition();
                enemyAI.SendMessage("StartIdle");

            }
            else
            {
                enemyTr.LookAt(nextPosition);
                enemyNav.destination = nextPosition;
                enemyNav.speed = moveSpeed;
            }

        }
        else
        {
            enemyNav.isStopped = true;
        }
    }

    public void SetNextPosition()
    {
        StopAllCoroutines();

        float newX = Mathf.Cos(Random.Range(0.0f, 360.0f)) 
            * moveRange + enemyTr.position.x;
        float newZ = Mathf.Sin(Random.Range(0.0f, 360.0f)) 
            * moveRange + enemyTr.position.z;

        nextPosition = new Vector3(newX, transform.position.y, newZ);

        StartCoroutine(ImpossiblePath());
    }

    public void TracePlayer(Vector3 playerPos)
    {

        isPatrol = false;
        enemyTr.LookAt(playerPos);
        enemyTr.Translate(Vector3.forward * traceSpeed * Time.deltaTime, Space.Self);

    }


    IEnumerator ImpossiblePath()
    {
        yield return impossiblePath; // 5초 동안 길을 찾지 못하면

        SetNextPosition(); // 다음 목적지로.
    }

}

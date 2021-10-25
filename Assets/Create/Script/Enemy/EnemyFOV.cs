using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{

    public float viewRange = 15.0f;

    [Range(0, 360)]
    public float viewAngle = 120.0f;


    public Transform rayPoint;
    Transform enemyTr;
    public Transform playerTr;
    int playerLayer;
    int obstacleLayer;
    int wallLayer;
    int layerMask;


    // Start is called before the first frame update
    void Start()
    {
        enemyTr = GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();

        playerLayer = LayerMask.NameToLayer("Player");
        obstacleLayer = LayerMask.NameToLayer("Obstacle");
        wallLayer = LayerMask.NameToLayer("Wall");
        layerMask = 1 << playerLayer | 1 << obstacleLayer | 1 << wallLayer;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 CirclePoint(float angle)
    {
        // 적 캐릭터의 Y 회전값을 더함.
        angle += transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 
            0.0f, Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    public bool isTracePlayer()
    {
        bool isTrace = false;

        Collider[] coll = Physics.OverlapSphere(enemyTr.position, viewRange, 1 << playerLayer);

        if(coll.Length == 1)
        {
            Vector3 dir = (playerTr.position - enemyTr.position).normalized;

            if(Vector3.Angle(enemyTr.forward, dir) < viewAngle * 0.5f)
            {
                isTrace = true;
            }
            else
            {
                enemyTr.LookAt(playerTr);
            }
        }
        return isTrace;
    }

    public bool isViewPlayer()
    {
        bool isView = false;

        RaycastHit ray;

        Vector3 dir = (playerTr.position - rayPoint.position).normalized;
        dir.y = 0.0f;
        if (Physics.Raycast(rayPoint.position, dir, out ray, viewRange, layerMask))
        {
            isView = (ray.collider.CompareTag("Player"));
        }

        return isView;
    }
}

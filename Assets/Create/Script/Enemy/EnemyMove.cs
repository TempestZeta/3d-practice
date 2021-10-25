using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    protected EnemyFOV enemyFOV;
    protected NavMeshAgent enemyNav;
    protected Vector3 nextPosition;
    public float traceSpeed;
    public float moveSpeed;
    public float enemySpeed;
    public bool isPatrol;
    public bool isTrace;

    virtual public void EnemyPatroll()
    {

    }

    virtual public void TraceTarget(Vector3 player)
    {

    }

    virtual public float CalculrateDist()
    {
        return 0.0f;
    }

    virtual public void Stop()
    {

    }

}

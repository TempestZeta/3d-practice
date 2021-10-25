using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyAttack : MonoBehaviour
{
    public float attRate;
    public MeshCollider weaponCol;
    protected Transform playerTr;
    protected Transform enemyTr;
    public bool isAttack;

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

    virtual public void Attack()
    {
        
    }

    virtual public IEnumerator AttackAction()
    {
        yield return null;
    }

    virtual public void StopAttack()
    {

    }

}

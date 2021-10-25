using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowEnemyAttack : EnemyAttack
{
    public Transform firePos;
    public GameObject arrowPf;

    private void Awake()
    {
        playerTr = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemyTr = GetComponent<Transform>();
        weaponCol.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Attack()
    {
        enemyTr.LookAt(playerTr);
        StartCoroutine(AttackAction());
    }

    public override IEnumerator AttackAction()
    {
        isAttack = true;

        yield return new WaitForSeconds(0.6f);

        GameObject tempArrow = Instantiate(arrowPf);
        tempArrow.transform.position = firePos.position;
        tempArrow.transform.rotation = firePos.rotation;
        tempArrow.GetComponent<BowEnemyCheck>().FireArrow();
        Destroy(tempArrow, 5.0f);

        yield return new WaitForSeconds(attRate);
        isAttack = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemyAttack : EnemyAttack
{
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
        if (isAttack)
        {
            //Quaternion rot = Quaternion.LookRotation(playerTr.position - enemyTr.position);
            //enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * 10.0f);
        }
    }

    public override void Attack()
    {
        StartCoroutine(AttackAction());
    }

    public override IEnumerator AttackAction()
    {
        isAttack = true;
        yield return new WaitForSeconds(attRate);
        isAttack = false;
    }

    public override void StopAttack()
    {
        StopAllCoroutines();
        isAttack = false;
        weaponCol.enabled = false;
    }

    // 애니메이션 이벤트로 무기 콜라이더 제어

    public void StartAttack()
    {
        weaponCol.enabled = true;
    }

    public void EndAttack()
    {
        weaponCol.enabled = false;
    }

}

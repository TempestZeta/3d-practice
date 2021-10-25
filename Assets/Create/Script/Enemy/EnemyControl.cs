using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public float enemyHP = 10.0f;

    Animator enemyAnim;

    WaitForSeconds ws;

    public enum EN_STATE
    {
        NONE = 0,
        IDLE,
        MOVE,
        TRACE,
        ATTACK,
        DAMAGE,
        DEATH
    };

    public EN_STATE state = EN_STATE.IDLE;

    readonly int hashMove   = Animator.StringToHash("isMove");
    readonly int hashAttack = Animator.StringToHash("Attack");
    readonly int whatAttack = Animator.StringToHash("isAttack");
    readonly int hashDamage = Animator.StringToHash("Damage");
    readonly int whatDamage = Animator.StringToHash("isDamage");
    readonly int hashDeath  = Animator.StringToHash("Death");
    readonly int whatDeath  = Animator.StringToHash("isDeath");

    EnemyMove eMove;
    EnemyAttack eAttack;
    EnemyFOV eFOV;

    Transform enemyTr;
    Transform playerTr;

    public int attackAniCount;
    public int damageAniCount;
    public int deateAniCount;

    private void Awake()
    {
        enemyAnim = GetComponent<Animator>();
        eMove = GetComponent<EnemyMove>();
        eAttack = GetComponent<EnemyAttack>();
        eFOV = GetComponent<EnemyFOV>();

        ws = new WaitForSeconds(0.3f);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(EnemyAction());
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyHP <= 0.0f)
        {
            state = EN_STATE.DEATH;
        }
    }

    public void EnemyDamage(float damage)
    {
        state = EN_STATE.DAMAGE;
        enemyHP -= damage;
    }

    IEnumerator CheckState()
    {
        while (true)
        {
            yield return ws;

            if (state == EN_STATE.DEATH)
            {
                yield break;
            }

            if(eMove.CalculrateDist() <= 3.0f)
            {
                if (eFOV.isViewPlayer())
                {
                    state = EN_STATE.ATTACK;
                }
            }
            else if(eMove.CalculrateDist() <= 15.0f)
            {
                if (eFOV.isViewPlayer())
                {
                    state = EN_STATE.TRACE;
                }
            }
            else
            {
                state = EN_STATE.IDLE;
            }
        }
    }

    IEnumerator EnemyAction()
    {
        while (true)
        {
            switch (state)
            {
                case EN_STATE.IDLE:
                    enemyAnim.SetBool(hashMove, false);
                    break;
                case EN_STATE.MOVE:
                    enemyAnim.SetBool(hashMove, true);
                    break;
                case EN_STATE.TRACE:
                    enemyAnim.SetBool(hashMove, true);
                    break;
                case EN_STATE.ATTACK:
                    break;
                case EN_STATE.DAMAGE:
                    enemyAnim.SetTrigger(hashDamage);
                    enemyAnim.SetInteger(whatDamage, Random.Range(0, damageAniCount));
                    break;
                case EN_STATE.DEATH:
                    enemyAnim.SetTrigger(hashDeath);
                    enemyAnim.SetInteger(whatDeath, Random.Range(0, deateAniCount));
                    state = EN_STATE.DEATH;
                    StopAllCoroutines();
                    GetComponent<CapsuleCollider>().enabled = false;
                    break;
            }

            yield return ws;
        }
    }


}

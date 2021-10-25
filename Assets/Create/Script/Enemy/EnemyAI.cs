using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    // 적 상태
    public enum EN_STATE
    {
        PATROL,
        TRACE,
        ATTACK,
        DIE
    }

    public EN_STATE state;

    public bool isDie = false;

    // 이동 관련
    EnemyMove enemyMove;
    Transform enemyTr;
    Transform playerTr;
    public Renderer renderer;
    public Material mat;
    float timeCheck = 0.0f;
    public float attDist;

    // 공격 관련
    EnemyAttack enemyAttack;
    public float enemyHP;

    EnemyFOV enemyFOV;

    WaitForSeconds ws;

    // 애니메이션

    public int attackAniCount;
    public int damageAniCount;
    public int deathAniCount;

    Animator anim;

    readonly int hashMove   = Animator.StringToHash("isMove");
    readonly int hashAttack = Animator.StringToHash("Attack");
    readonly int whatAttack = Animator.StringToHash("isAttack");
    readonly int hashDamage = Animator.StringToHash("Damage");
    readonly int whatDamage = Animator.StringToHash("isDamage");
    readonly int hashDeath  = Animator.StringToHash("Death");
    readonly int whatDeath  = Animator.StringToHash("isDeath");
    readonly int speed      = Animator.StringToHash("Speed");

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if(player != null)
        {
            playerTr = player.GetComponent<Transform>();
        }

        enemyTr = GetComponent<Transform>();

        ws = new WaitForSeconds(0.3f);

        enemyMove = GetComponent<EnemyMove>();
        enemyAttack = GetComponent<EnemyAttack>();
        enemyFOV = GetComponent<EnemyFOV>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(EnemyAction());
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyHP <= 0.0f)
        {
            if (!isDie)
            {
                StopAllCoroutines();
                enemyMove.Stop();
                enemyAttack.isAttack = false;
                isDie = true;
                GetComponent<CapsuleCollider>().enabled = false;
                renderer.material = mat;
                StartCoroutine(DissolveEnemy());

            }
        }
        else
        {
            anim.SetFloat(speed, enemyMove.enemySpeed);
        }
    }

    IEnumerator CheckState()
    {
        yield return new WaitForSeconds(1.0f);

        while (true)
        {
            if (state == EN_STATE.DIE) yield break;

            float dist = Vector3.Distance(enemyTr.position, playerTr.position);

            if(dist <= attDist)
            {
                if (enemyFOV.isViewPlayer())
                {
                    state = EN_STATE.ATTACK;
                }
                else
                {
                    state = EN_STATE.TRACE;
                }
            }
            else if(enemyFOV.isTracePlayer())
            {
                state = EN_STATE.TRACE;
            }
            else
            {
                state = EN_STATE.PATROL;
            }

            yield return ws;
        }
    }

    IEnumerator EnemyAction()
    {
        while(true)
        {
            yield return ws;

            switch (state)
            {
                case EN_STATE.PATROL:
                    enemyMove.isPatrol = true;
                    anim.SetBool(hashMove, true);
                    break;
                case EN_STATE.TRACE:
                    enemyMove.TraceTarget(playerTr.position);
                    anim.SetBool(hashMove, true);
                    break;
                case EN_STATE.ATTACK:
                    anim.SetBool(hashMove, false);
                    enemyMove.Stop();
                    if (!enemyAttack.isAttack)
                    {
                        anim.SetTrigger(hashAttack);
                        if(attackAniCount == 0)
                        {
                            anim.SetInteger(whatAttack, 0);
                        }
                        else
                        {
                            anim.SetInteger(whatAttack, Random.Range(0, attackAniCount));
                        }
                        enemyAttack.Attack();
                    }
                    break;
                case EN_STATE.DIE:
                    enemyMove.Stop();
                    break;
            }
        }
    }

    public void EnemyDamage(float damage)
    {
        enemyHP -= damage;

        if (enemyHP > 0.0f)
        {
            anim.SetTrigger(hashDamage);
            anim.SetInteger(whatDamage, Random.Range(0, damageAniCount));
        }
        else
        {
            anim.SetTrigger(hashDeath);
            anim.SetInteger(whatDeath, Random.Range(0, deathAniCount));
        }
    }

    IEnumerator DissolveEnemy()
    {
        while (true)
        {
            timeCheck += Time.deltaTime;
            renderer.material.SetFloat("_DissolveAmount", timeCheck);

            if (timeCheck > 1.0f)
            {
                Destroy(gameObject);
                yield break;
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}

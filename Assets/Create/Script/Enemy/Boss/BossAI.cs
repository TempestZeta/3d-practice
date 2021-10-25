using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : NewEnemyAI
{
    private void Awake()
    {
        playerTr = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemyTr = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        enemyFov = GetComponent<EnemyFOV>();
        enemyMove = GetComponent<NewEnemyMove>();
        enemyAttack = GetComponent<EnemyAttack>();
        audio = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyHP += PlayerPrefs.GetInt("Difficult") * 5;
        isAlive = false;
        //StartIdle();
    }

    private void OnEnable()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                EnemyDistroy();
            }

            anim.SetFloat(moveSpeed, enemyMove.enemySpeed);

            float playerDist = Vector3.Distance(playerTr.position, enemyTr.position);

            if (playerDist < attDist)
            {
                if (enemyFov.isTracePlayer() && enemyFov.isViewPlayer())
                {
                    state = STATE.ATTACK;
                }
            }
            else
            {
                if (enemyFov.isTracePlayer() && enemyFov.isViewPlayer())
                {
                    state = STATE.TRACE;
                }
                else
                {
                    state = STATE.PATROL;
                }
            }

            switch (state)
            {
                case STATE.IDLE:
                    anim.SetBool(hashMove, false);
                    break;
                case STATE.PATROL:
                    enemyMove.isPatrol = true;
                    enemyMove.isTrace = false;
                    anim.SetBool(hashMove, true);
                    break;
                case STATE.TRACE:
                    if (!enemyAttack.isAttack)
                    {
                        anim.SetBool(hashMove, true);
                        StartTrace();
                    }
                    break;
                case STATE.ATTACK:
                    StopAllCoroutines();
                    anim.SetBool(hashMove, false);
                    enemyMove.isPatrol = false;
                    if (!enemyAttack.isAttack)
                    {
                        enemyAttack.Attack();
                        anim.SetTrigger(hashAttack);
                        anim.SetInteger(whatAttack, Random.Range(0, attackAniCount));
                    }
                    break;
            }

        }
    }

    public override void StartIdle()
    {
        StartCoroutine(IdleCourotine());
    }

    public override IEnumerator IdleCourotine()
    {
        enemyMove.isPatrol = false;
        state = STATE.IDLE;

        float waitTime = Random.Range(1.0f, 5.0f);
        yield return new WaitForSeconds(waitTime);

        enemyMove.isPatrol = true;
        state = STATE.PATROL;
    }

    public override void StartTrace()
    {
        StopAllCoroutines();
        enemyMove.isPatrol = true;
        enemyMove.TracePlayer(playerTr.position);
    }

    public override void EnemyDamage(float damage)
    {
        enemyHP -= damage;

        //hit.transform.position = hitPoint;
        hit.EffectPlay(hit.transform.position);


        if (enemyHP > 0.0f)
        {
            anim.SetTrigger(hashDamage);
            enemyTr.LookAt(playerTr);
        }
        else
        {
            audio.PlayOneShot(deathSound, PlayerPrefs.GetFloat("Volume"));
            anim.SetTrigger(hashDeath);
            isAlive = false;
            enemyMove.isPatrol = false;
            enemyAttack.StopAttack();
            GetComponent<CapsuleCollider>().enabled = false;
        }
    }
    public override void BossAppear()
    {
        anim.SetTrigger("isAlive");
        isAlive = true;
        GetComponent<CapsuleCollider>().enabled = true;
    }

    public override void EnemyDistroy()
    {
        EnemyDamage(100.0f);
    }
}

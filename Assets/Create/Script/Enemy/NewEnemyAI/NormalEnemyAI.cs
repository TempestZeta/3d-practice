using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemyAI : NewEnemyAI
{
    private void Awake()
    {
        playerTr = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemyTr = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        enemyFov = GetComponent<EnemyFOV>();
        enemyMove = GetComponent<NewEnemyMove>();
        enemyAttack = GetComponent<EnemyAttack>();
        enemyItem = GetComponent<EnemyItemDrop>();
        audio = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyHP += PlayerPrefs.GetInt("Difficult") * 5;
        timeCheck = 0.0f;
        isAlive = true;
        StartIdle();
    }

    private void OnEnable()
    {

    }

    // Update is called once per frame
    void Update()
    {
        EnemyDistroy();

        if (isAlive)
        {
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
            anim.SetInteger(whatDamage, Random.Range(0, damageAniCount));
            enemyTr.LookAt(playerTr);
        }
        else
        {
            audio.PlayOneShot(deathSound, PlayerPrefs.GetFloat("Volume"));
            anim.SetTrigger(hashDeath);
            anim.SetInteger(whatDeath, Random.Range(0, deateAniCount));
            isAlive = false;
            enemyMove.isPatrol = false;
            enemyAttack.StopAttack();
            GetComponent<CapsuleCollider>().enabled = false;
            enemyItem.DropItem();
            StartCoroutine(FadeEnemy());
        }
    }

    public override IEnumerator FadeEnemy()
    {
        renderer.material = diffuseMat;

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

    public override void EnemyDistroy()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            EnemyDamage(100.0f);
        }
    }
}

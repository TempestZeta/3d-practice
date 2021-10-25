using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowEnemyAI : NewEnemyAI
{
    private void Awake()
    {
        playerTr = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemyTr = GetComponent<Transform>();
        enemyMove = GetComponent<NewEnemyMove>();
        enemyFov = GetComponent<EnemyFOV>();
        enemyAttack = GetComponent<EnemyAttack>();
        anim = GetComponent<Animator>();
        enemyItem = GetComponent<EnemyItemDrop>();
        audio = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyHP += PlayerPrefs.GetInt("Difficult") * 5;
        isAlive = true;
        StartIdle();
    }

    // Update is called once per frame
    void Update()
    {
        EnemyDistroy();

        if (isAlive)
        {
            anim.SetFloat(moveSpeed, enemyMove.enemySpeed);

            float dist = Vector3.Distance(enemyTr.position, playerTr.position);

            if(dist < attDist)
            {
                if (enemyFov.isViewPlayer())
                {
                    state = STATE.ATTACK;
                }
            }

            switch (state)
            {
                case STATE.IDLE:
                    anim.SetBool(hashMove, false);
                    break;
                case STATE.PATROL:
                    enemyMove.isPatrol = true;
                    anim.SetBool(hashMove, true);
                    break;
                case STATE.ATTACK:
                    StopAllCoroutines();
                    anim.SetBool(hashMove, false);
                    enemyMove.isPatrol = false;
                    if (!enemyAttack.isAttack)
                    {
                        enemyAttack.Attack();
                        anim.SetTrigger(hashAttack);
                        anim.SetInteger(whatAttack, 0);
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

        float waitTime = Random.Range(2.0f, 10.0f);
        yield return new WaitForSeconds(waitTime);

        enemyMove.isPatrol = true;
        state = STATE.PATROL;
    }

    public override void EnemyDamage(float damage)
    {
        enemyHP -= damage;
        hit.EffectPlay(hit.transform.position);

        if (enemyHP > 0.0f)
        {
            anim.SetTrigger(hashDamage);
            anim.SetInteger(whatDamage, Random.Range(0, damageAniCount));
        }
        else
        {
            audio.PlayOneShot(deathSound, PlayerPrefs.GetFloat("Volume"));
            anim.SetTrigger(hashDeath);
            anim.SetInteger(whatDeath, Random.Range(0, deateAniCount));
            isAlive = false;
            enemyMove.isPatrol = false;
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

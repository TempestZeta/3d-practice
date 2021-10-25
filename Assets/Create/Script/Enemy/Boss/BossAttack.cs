using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{

    public bool isAttack;

    public CapsuleCollider weaponCol;

    public struct AttackBonus
    {
        public int attIndex;
        public int score;
        public float damage;
    }

    public AttackBonus[] atBonus;

    float attRate;

    // Start is called before the first frame update
    void Start()
    {
        isAttack = false;
        weaponCol.enabled = false;
        atBonus = new AttackBonus[4];
        for(int i = 0; i < 4; i++)
        {
            atBonus[i].attIndex = i;
            atBonus[i].score = 0;
            atBonus[i].damage = 30.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int AttackScoreCheck(float dist)
    {


        isAttack = true;

        if (dist > 2.0f)
        {
            if (atBonus[0].score > atBonus[1].score)
            {
                atBonus[0].score -= 10;
                attRate = 2.0f;
                return atBonus[0].attIndex;
            }
            else
            {
                atBonus[1].score -= 10;
                attRate = 3.0f;
                return atBonus[1].attIndex;
            }
        }
        else
        {
            if (atBonus[2].score > atBonus[3].score)
            {
                atBonus[2].score -= 10;
                attRate = 1.0f;
                return atBonus[2].attIndex;
            }
            else
            {
                atBonus[3].score -= 10;
                attRate = 1.0f;
                return atBonus[3].attIndex;
            }
        }
    }
    
    public void StartAttack()
    {
        weaponCol.enabled = true;
    }

    public void EndAttack()
    {
        weaponCol.enabled = false;
        StartCoroutine(AttackRate(attRate));
    }

    IEnumerator AttackRate(float rate)
    {
        yield return new WaitForSeconds(rate);

        isAttack = false;
    }
}

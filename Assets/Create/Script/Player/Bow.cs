using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Attack
{

    public Transform firePos;
    public GameObject arrow;

    Animator anim;

    readonly int hashAttack = Animator.StringToHash("Attack2");

    // Start is called before the first frame update
    void Start()
    {
        attRate = 1.5f;
        attDamage = 10.0f;
        skillRate = 2.2f;
        skillDamage = 3.0f;
        attStm = 10.0f;
        skillStm = 10.0f;

        isAttack = false;
        weaponColl.enabled = false;
        hitCheck = weaponColl.GetComponent<WeaponCheck>();
        anim = GetComponent<Animator>();
        hitCheck.ohs = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>().attacks[2];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool AttackAction()
    {
        if (!isAttack)
        {
            if (GameManager.singleton.playerStm > attStm)
            {
                realDamage = attDamage;

                StartCoroutine(AttackRate());

                GameManager.singleton.playerStm -= attStm;

                return true;
            }
        }

        return false;
    }

    public override bool SkillAction()
    {
        if (!isAttack)
        {
            if (GameManager.singleton.playerStm > skillStm)
            {
                realDamage = skillDamage;

                StartCoroutine(SkillRate());

                GameManager.singleton.playerStm -= skillStm;

                return true;
            }
        }

        return false;
    }

    public override IEnumerator AttackRate()
    {
        isAttack = true;
        yield return new WaitForSeconds(attRate);
        anim.SetTrigger(hashAttack);
        GameObject tempArrow = Instantiate(arrow);
        tempArrow.transform.SetParent(null);
        tempArrow.transform.position = firePos.position;
        tempArrow.transform.rotation = firePos.rotation;
        //tempArrow.GetComponent<ArrowCheck>().ArrowFire();
        isAttack = false;
    }

    public override IEnumerator SkillRate()
    {
        isAttack = true;
        weaponColl.enabled = true;

        yield return new WaitForSeconds(skillRate);

        weaponColl.enabled = false;
        isAttack = false;
    }

    public override void Damage(Collider other)
    {
        other.SendMessage("EnemyDamage", realDamage);
    }

}

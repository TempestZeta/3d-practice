using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoHandSword : Attack
{
    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        // 양손검 관련 수치 조정 필요
        attDamage = 7.0f;
        skillDamage = 15.0f;
        attStm = 15.0f;
        skillStm = 20.0f;

        isAttack = false;
        weaponColl.enabled = false;
        hitCheck = weaponColl.GetComponent<WeaponCheck>();
        hitCheck.ohs = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>().attacks[0];
        attAudio = GetComponent<AudioSource>();

        effectList = new List<XftWeapon.XWeaponTrail>();
        hitCheck.GetComponentsInChildren<XftWeapon.XWeaponTrail>(effectList);
        EffectOnOff(false);

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

                attAudio.PlayOneShot(attSound, PlayerPrefs.GetFloat("Volume"));

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

                attAudio.PlayOneShot(attSound, PlayerPrefs.GetFloat("Volume"));

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
        EffectOnOff(true);
        yield return new WaitForSeconds(0.5f);
        weaponColl.enabled = true;
       
        yield return new WaitForSeconds(0.3f);
        weaponColl.enabled = false;
        
        yield return new WaitForSeconds(0.3f);
        isAttack = false;
        EffectOnOff(false);
    }

    public override IEnumerator SkillRate()
    {
        isAttack = true;
        EffectOnOff(true);
        yield return new WaitForSeconds(0.7f);
        weaponColl.enabled = true;
       
        yield return new WaitForSeconds(0.4f);
        weaponColl.enabled = false;
        
        yield return new WaitForSeconds(0.3f);
        isAttack = false;
        EffectOnOff(false);
    }

    public override void Damage(Collider other)
    {
        other.SendMessage("EnemyDamage", realDamage);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneHandSword : Attack
{
    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        attRate = 1.5f;
        attDamage = 5.0f;
        skillRate = 2.2f;
        skillDamage = 7.0f;
        attStm = 5.0f;
        skillStm = 20.0f;
        isAttack = false;
        weaponColl.enabled = false;
        hitCheck = weaponColl.GetComponent<WeaponCheck>();
        attAudio = GetComponent<AudioSource>();
        hitCheck.ohs = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>().attacks[1];

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
            if(GameManager.singleton.playerStm > attStm)
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
        
        yield return new WaitForSeconds(0.2f);
        EffectOnOff(false);
        isAttack = false;
        
    }

    public override IEnumerator SkillRate()
    {
        isAttack = true;
        EffectOnOff(true);
        yield return new WaitForSeconds(0.7f);

        weaponColl.enabled = true;
        
        yield return new WaitForSeconds(0.7f);

        weaponColl.enabled = false;
        
        yield return new WaitForSeconds(0.2f);
        EffectOnOff(false);
        isAttack = false;
        
    }

    public override void Damage(Collider other)
    {
        other.SendMessage("EnemyDamage", realDamage);
    }
}

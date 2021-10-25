using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public CapsuleCollider weaponColl; // 공격 무기
    public AudioClip attSound; // 공격 소리

    protected WeaponCheck hitCheck; // 공격 적중 여부
    protected AudioSource attAudio; // 오디오 소스
    protected float attRate; // 공격 주기
    protected float attDamage;
    protected float skillRate; // 스킬 주기
    protected float skillDamage;
    protected float attStm; // 공격시 소모되는 스태미나
    protected float skillStm; // 스킬시 소모되는 스태미나

    protected float realDamage; // 실제 적 컴포넌트에 전해줄 대미지

    protected bool isAttack; // 공격 했는지 여부

    public bool IsAttack
    {
        get { return isAttack; }
        set { isAttack = value; }
    }

    // 이펙트 관련

    protected List<XftWeapon.XWeaponTrail> effectList;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual bool AttackAction()
    {
        return false;
    }

    public virtual bool SkillAction()
    {
        return false;
    }

    public virtual IEnumerator AttackRate()
    {
        yield return null;
    }

    public virtual IEnumerator SkillRate()
    {
        yield return null;
    }

    public virtual void Damage(Collider other)
    {

    }

    public void EffectOnOff(bool b)
    {
        for (int i = 0; i < effectList.Count; i++)
        {
            effectList[i].enabled = b;
        }
    }
}

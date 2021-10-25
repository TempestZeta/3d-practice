using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewEnemyAI : MonoBehaviour, EnemyInterface
{
    public float enemyHP;

    public enum STATE
    {
        IDLE = 0,
        PATROL,
        TRACE,
        ATTACK
    }

    public STATE state;
    public float attDist;

    protected bool isAlive;
    
    protected Transform playerTr;
    protected Transform enemyTr;
    protected NewEnemyMove enemyMove;
    protected EnemyFOV enemyFov;
    protected EnemyAttack enemyAttack;
    protected EnemyItemDrop enemyItem;

    protected WaitForSeconds ws = new WaitForSeconds(0.3f);

   

    // 애니메이션 관련 

    protected Animator anim;

    protected readonly int hashMove = Animator.StringToHash("isMove");
    protected readonly int hashAttack = Animator.StringToHash("Attack");
    protected readonly int whatAttack = Animator.StringToHash("isAttack");
    protected readonly int hashDamage = Animator.StringToHash("Damage");
    protected readonly int whatDamage = Animator.StringToHash("isDamage");
    protected readonly int hashDeath = Animator.StringToHash("Death");
    protected readonly int whatDeath = Animator.StringToHash("isDeath");
    protected readonly int moveSpeed = Animator.StringToHash("Speed");

    // 사망시 머티리얼 변경
    public SkinnedMeshRenderer renderer;
    public Material diffuseMat;
    protected float timeCheck;

    // 공격 애니메이션 갯수
    public int attackAniCount;
    public int damageAniCount;
    public int deateAniCount;

    // 피격 효과
    public HitEffectPlay hit;

    // 사망시 소리
    public AudioClip deathSound;
    protected AudioSource audio;

    private void Awake()
    {   
        
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    virtual public void StartIdle()
    {
        
    }

    virtual public IEnumerator IdleCourotine()
    {
        yield return null;
    }

    virtual public void StartTrace()
    {

    }

    virtual public void EnemyDamage(float damage)
    {
        
    }

    virtual public IEnumerator FadeEnemy()
    {
        yield return null;
    }

    public int Count()
    {
        if(enemyHP <= 0.0f)
        {
            return 1;
        }
        return 0;
    }

    public virtual void EnemyDistroy()
    {

    }

    public virtual void BossAppear()
    {

    }

}

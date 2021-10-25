using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject weapon;

    RaycastHit pAttack;

    public float attDist = 0.5f;

    bool _isAttack;

    public bool IsAttack
    {
        get { return _isAttack; }
        set { _isAttack = value; }
    }

    float _attRate = 1.5f;
    float _timeCheck = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_isAttack)
        {
            _timeCheck += Time.deltaTime;

            if(_timeCheck >= _attRate)
            {
                _timeCheck = 0.0f;
                _isAttack = false;
            }
        }
    }

    //void AttackCheck()
    //{
    //    Debug.DrawRay(weapon.transform.position, transform.forward * attDist, Color.blue, 1.0f);

    //    if (Physics.Raycast(weapon.transform.position, transform.forward, out pAttack, attDist))
    //    {
    //        if (pAttack.collider.gameObject.tag == "Enemy")
    //        {
    //            EnemyControl ec = pAttack.collider.gameObject.GetComponent<EnemyControl>();
    //            ec.EnemyDamage();
    //        }

    //    }
    //}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    bool _isRolling;

    public bool IsRoll
    {
        get { return _isRolling; }
        set { _isRolling = value; }
    }

    float rollRate = 2.0f;
    float timeCheck = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_isRolling)
        {
            timeCheck += Time.deltaTime;

            if(timeCheck >= rollRate)
            {
                timeCheck = 0.0f;
                _isRolling = false;
            }
        }
    }
}

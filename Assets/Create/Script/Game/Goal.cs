using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public TriggerCheck tc;

    string noneGoal;

    string boss;

    public bool isClear;

    public bool isBoss;

    // Start is called before the first frame update
    void Start()
    {
        isClear = false;
        isBoss = false;

        noneGoal = "봉인이 풀리지 않았다.";
        boss = "그가 눈을 떴다.";
    }

    // Update is called once per frame
    void Update()
    {
        if (tc.isTrue)
        {
            if (!isClear)
            {
                if (isBoss)
                {
                    UIMgr.uiMgr.ShowMessage(boss);
                }
                else
                {
                    UIMgr.uiMgr.ShowMessage(noneGoal);
                }
                
            }
            else
            {
                // 게임 클리어
                UIMgr.uiMgr.ShowMessage("봉인의 서를 손에 넣었다.");
                StartCoroutine(GameManager.singleton.Clear());
            }
        }
    }
}

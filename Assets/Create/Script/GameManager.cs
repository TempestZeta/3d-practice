using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using LitJson;

public class ItemInfo
{
    public int itemIndex;
    public string name;
    public string itemDis;
}

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;

    public Sprite[] weaponImage;

    GameObject[] gameObjects;

    public float playerHP;
    public float playerStm;

    public int archerCheck = 0;

    //public BossAI boss;
    public Goal goal;

    List<ItemInfo> item = new List<ItemInfo>();

    public JsonData itemInfomation;

    public AudioClip[] backMusic;
    AudioSource audio;

    // 엔딩
    public bool isClear;
    public Image fadeOut;
    float fadeCheck;
    bool isBoss;

    private void Awake()
    {
        gameObjects = GameObject.FindGameObjectsWithTag("Enemy");

        //enemy = new EnemyInterface[gameObjects.Length];

        //for (int i = 0; i < gameObjects.Length; i++)
        //{
        //    enemy[i] = gameObjects[i].GetComponent<EnemyInterface>();
        //}

        singleton = this;
        playerHP = 100.0f;
        playerStm = 100.0f;
        string tempString = File.ReadAllText(Application.dataPath + "/resourse/itemData.json", System.Text.Encoding.UTF8);
        itemInfomation = JsonMapper.ToObject(tempString);
        isBoss = false;
        isClear = false;
        audio = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //audio.PlayOneShot(backMusic[0], PlayerPrefs.GetFloat("Volume"));
        SoundManager.soundMgr.PlaySound(audio, MusicKinds.BACKGROUND);
        
        //SaveData();
        //Debug.Log(jsonData[0]["name"]);
        //Debug.Log(jsonData[1]["name"]);
        //Debug.Log(jsonData[2]["name"]);
        //Debug.Log(jsonData[3]["name"]);
    }

    // Update is called once per frame
    void Update()
    {
        int remainEnemy = 0;

        for (int i = 0; i < gameObjects.Length; i++)
        {
            if (gameObjects[i] == null)
            {
                remainEnemy++;
            }

            if(remainEnemy == 10)
            {
                goal.isClear = true;
            }
            else if(remainEnemy == 5)
            {
                if (!isBoss)
                {
                    SoundManager.soundMgr.PlaySound(audio, MusicKinds.BOSS);
                    isBoss = true;
                }
                
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void SaveData()
    {
        ItemInfo twoHand = new ItemInfo();
        ItemInfo oneHand = new ItemInfo();
        ItemInfo bow = new ItemInfo();
        ItemInfo portion = new ItemInfo();
        ItemInfo arrow = new ItemInfo();

        twoHand.itemIndex = 0;
        twoHand.name = "바스타드 소드";
        twoHand.itemDis = "숙련된 전사들이 사용하는 길다란 장검.";
        item.Add(twoHand);

        oneHand.itemIndex = 1;
        oneHand.name = "브로드 소드";
        oneHand.itemDis = "한 손으로 다룰 수 있는 검과 방패.";
        item.Add(oneHand);

        bow.itemIndex = 2;
        bow.name = "롱 보우";
        bow.itemDis = "원거리의 적도 공격할 수 있는 활";
        item.Add(bow);

        portion.itemIndex = 3;
        portion.name = "에스트병";
        portion.itemDis = "생명력을 담은 물약.";
        item.Add(portion);

        arrow.itemIndex = 4;
        arrow.name = "화살";
        arrow.itemDis = "그럭저럭 멀리 날아가는 화살";
        item.Add(arrow);

        JsonData jsonData = JsonMapper.ToJson(item);

        File.WriteAllText(Application.dataPath + "/resourse/itemData.json", jsonData.ToString(), System.Text.Encoding.UTF8);
    }

    public IEnumerator Clear()
    {
        while (true)
        {
            fadeOut.color = new Vector4(0.0f, 0.0f, 0.0f, fadeCheck);

            if(fadeCheck < 1.0f)
            {
                fadeCheck += Time.deltaTime;
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                SceneManager.LoadScene(2);
                yield break;
            }
        }
    }

}

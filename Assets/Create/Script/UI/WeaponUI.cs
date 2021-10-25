using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{

    [System.Serializable] // 갱신될 무기 이미지
    public class WeaponSlot
    {
        public Image img; // 이미지 (변환의 주체)
        public int index; // 무기 번호 - 0. TwoHand 1. OneHand 2. Bow
        public int currIndex;
        public int nextIndex;

        public void SetNextIdx()
        {
            currIndex = nextIndex;
            nextIndex = (nextIndex + 1) % 3;
        }

    }

    [System.Serializable] // 갱신될 필요 없는 '고정 위치'
    public class ImagePosition
    {
        public int index; // 몇 번째 칸인가

        public Vector3 rectPos; // 칸의 위치는 어디인가
                
        public float scale; // 칸의 크기는 얼마인가

        public void Init(Image img, int i, float f) // 생성자
        {
            index = i;
            scale = f;
            rectPos = img.rectTransform.position;
        }
    }

    public WeaponSlot[] weaponSlot;

    ImagePosition[] imgPos;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        imgPos = new ImagePosition[3];

        for (int i = 0; i < weaponSlot.Length; i++)
        {
            ImagePosition tempPos = new ImagePosition();
            tempPos.Init(weaponSlot[i].img, i, i == 0 ? 1.0f : 0.5f);
            imgPos[i] = tempPos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            for(int i = 0; i < 3; i++)
            {
                StartCoroutine(WeaponChange(i));
            }
        }
    }

    IEnumerator WeaponChange(int index)
    {
        while (true)
        {
            Vector3 dir = (imgPos[weaponSlot[index].nextIndex].rectPos - weaponSlot[index].img.rectTransform.position); // 방향
            float scaleDist = (imgPos[weaponSlot[index].nextIndex].scale - imgPos[weaponSlot[index].currIndex].scale) * Time.deltaTime;

            if (dir.sqrMagnitude <= 0.5f * 0.5f) // 도착 시 위치 고정
            {
                weaponSlot[index].img.rectTransform.position = imgPos[weaponSlot[index].nextIndex].rectPos;
                weaponSlot[index].img.rectTransform.localScale = 
                    new Vector3(imgPos[weaponSlot[index].nextIndex].scale, imgPos[weaponSlot[index].nextIndex].scale, imgPos[weaponSlot[index].nextIndex].scale);
                weaponSlot[index].img.color = 
                    new Vector4(1.0f, 1.0f, 1.0f, imgPos[weaponSlot[index].nextIndex].scale);
                weaponSlot[index].SetNextIdx();
                yield break;
            }
            else // 도착 못하면 서서히 변하게
            {
                weaponSlot[index].img.rectTransform.Translate(dir * 10.0f * Time.deltaTime);

                if (scaleDist != 0.0f)
                {
                    if(Mathf.Abs(weaponSlot[index].img.color.a - imgPos[weaponSlot[index].nextIndex].scale) > 0.001f)
                    {
                        float scaleChange = weaponSlot[index].img.rectTransform.localScale.x + scaleDist;

                        weaponSlot[index].img.rectTransform.localScale = Vector3.Lerp(weaponSlot[index].img.rectTransform.localScale,
                            new Vector3(imgPos[weaponSlot[index].nextIndex].scale, imgPos[weaponSlot[index].nextIndex].scale, imgPos[weaponSlot[index].nextIndex].scale), 0.1f);

                        weaponSlot[index].img.color = new Vector4(1.0f, 1.0f, 1.0f, scaleChange);
                    }
                }
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}



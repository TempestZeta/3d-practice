using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MusicKinds
{
    BACKGROUND = 0,
    BOSS
}

public struct MusicCompare : IEqualityComparer<MusicKinds>
{
    public bool Equals(MusicKinds a, MusicKinds b)
    {
        return a == b;
    }

    public int GetHashCode(MusicKinds obj)
    {
        return (int)obj;
    }
}

public class SoundManager : MonoBehaviour
{
    [System.Serializable]
    public class MusicInfo
    {
        public MusicKinds kind;
        public AudioClip audioClip;
    }

    public static SoundManager soundMgr;

    public List<MusicInfo> listMusic;

    public Dictionary<MusicKinds, AudioClip> dicSound;

    public float volume;

    private void Awake()
    {
        soundMgr = this;
        AddSound();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        volume = PlayerPrefs.GetFloat("Volume");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddSound()
    {
        dicSound = new Dictionary<MusicKinds, AudioClip>(new MusicCompare());

        for (int i = 0; i < listMusic.Count; i++)
        {
            dicSound.Add(listMusic[i].kind, listMusic[i].audioClip);
        }
    }

    public void PlaySound(AudioSource audio, MusicKinds kinds)
    {
        audio.PlayOneShot(dicSound[kinds], volume);
    }
}

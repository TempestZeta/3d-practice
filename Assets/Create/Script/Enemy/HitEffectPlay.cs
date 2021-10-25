using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffectPlay : MonoBehaviour
{
    List<ParticleSystem> hitEffect;

    WaitForSeconds wait = new WaitForSeconds(0.5f);

    private void Awake()
    {
        hitEffect = new List<ParticleSystem>();
        GetComponentsInChildren<ParticleSystem>(hitEffect);
    }

    // Start is called before the first frame update
    void Start()
    {
        EffectStop();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            EffectPlay(transform.position);
        }
    }

    public void EffectPlay(Vector3 pos)
    {
        transform.position = pos;
        foreach(ParticleSystem effect in hitEffect)
        {
            effect.Play();
        }
        StartCoroutine(PlayAndStop());
    }

    public void EffectStop()
    {
        foreach (ParticleSystem effect in hitEffect)
        {
            effect.Stop();
        }
    }

    IEnumerator PlayAndStop()
    {
        yield return wait;

        EffectStop();
    }

}

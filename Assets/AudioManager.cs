using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource sfxSource;

    public AudioClip lightAttack;
    public AudioClip heavyAttack;
    public AudioClip lightHit;
    public AudioClip heavyHit;
    public AudioClip finalHit;
    public enum SFX
    {
        LightAtk, HeavyAtk, LightHit, HeavyHit, FinalHit
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlaySound(SFX sfx)
    {
        if(sfx == SFX.FinalHit)
        {
            sfxSource.PlayOneShot(finalHit);
        }
    }
}

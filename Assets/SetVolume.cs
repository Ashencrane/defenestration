using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    [SerializeField]
    AudioMixer am;
    // Start is called before the first frame update
    void Start()
    {
        am.SetFloat("Music_Vol", ((Mathf.Log10(OptionSettings.MusicSetting * 20) - 0.5f) * 10f));
    }

}

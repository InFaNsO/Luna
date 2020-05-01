using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioMixer _audioMixer;
 
     private void Awake()
    {
        Debug.Log("[AudioManager] Awake");
        DontDestroyOnLoad(gameObject);
    }
      public void SetMasterVolume(float vol)
    {
        _audioMixer.SetFloat("Master_Volume", vol);
    }
    public   void SetMusicVolume(float vol)
    {
        _audioMixer.SetFloat("Music_Volume", vol);

    }
    public   void SetSFXVolume(float vol)
    {
        _audioMixer.SetFloat("SFX_Volume", vol);

    }


}

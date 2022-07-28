using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AudioSetting : ScriptableObject
{
    public const float defaultMusicVolume = 0.5f;
    public const float defaultSFXVolume = 0.5f;

    public float currentMusicVolume;
    public float currentSFXVolume;

    public void ResetVolume()
    {
        currentMusicVolume = defaultMusicVolume;
        currentSFXVolume = defaultSFXVolume;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuSettings : MonoBehaviour
{
    public GameObject settings;
    public GameObject MainMenu;
    public GameObject Customize;
    public GameObject helpDesk;
    public Slider musicSlider = null;
    public AudioSource audioSource = null;
    public Slider sfxlider = null;
    public AudioSource sfxSource = null;
    float savedVolume = 5.0f;
    //public float currentVolume = 5.0f;
    float sfxSavedVolume = 5.0f;
    //public float sfxCurrentVolume = 1.0f;

    public AudioClip openSound = null;
    public AudioClip closeSound = null;

    public AudioSetting audioSetting;



    // Start is called before the first frame update
    public void Play()
    {
        SceneManager.LoadScene("TJay Test");
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");

    }
    public void LoadTheScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    public void Settings()
    {
        MainMenu.SetActive(false);
        settings.SetActive(true);
    }
    public void SettingsBack()
    {
        MainMenu.SetActive(true);
        settings.SetActive(false);
    }
    public void CustomizeBack()
    {
        MainMenu.SetActive(true);
        Customize.SetActive(false);
    }
    public void Quit()
    {
        Application.Quit();
    }
    private void Start()
    {
        savedVolume = audioSetting.currentMusicVolume;
        sfxSavedVolume = audioSetting.currentSFXVolume;
        if (musicSlider != null)
        {
            if (audioSource != null)
            {
                //musicSlider.value = audioSetting.currentMusicVolume;
                musicSlider.SetValueWithoutNotify(audioSetting.currentMusicVolume);
                audioSource.volume = audioSetting.currentMusicVolume;
            }
        }

        if (sfxlider != null)
        {
            if (sfxSource != null)
            {
                //sfxlider.value = audioSetting.currentSFXVolume;
                sfxlider.SetValueWithoutNotify(audioSetting.currentSFXVolume);
                sfxSource.volume = audioSetting.currentSFXVolume;
            }
        }
    }
    public void UpdateVolumes()
    {
        if (musicSlider != null)
        {
            if (audioSource != null)
            {
                if (audioSetting.currentMusicVolume != musicSlider.value)
                {
                    audioSetting.currentMusicVolume = musicSlider.value;
                    if (audioSource.volume != audioSetting.currentMusicVolume)
                        audioSource.volume = audioSetting.currentMusicVolume;
                }

            }
        }
    }

    public void UpdateSFX()
    {
        if (sfxlider != null)
        {
            if (sfxSource != null)
            {
                if (audioSetting.currentSFXVolume != sfxlider.value)
                {
                    audioSetting.currentSFXVolume = sfxlider.value;
                    if (sfxSource.volume != audioSetting.currentSFXVolume)
                        sfxSource.volume = audioSetting.currentSFXVolume;
                }

            }
        }
    }

    public void Cancel()
    {
        audioSource.volume = savedVolume;
        musicSlider.value = savedVolume;

        sfxSource.volume = sfxSavedVolume;
        sfxlider.value = sfxSavedVolume;
    }

    public void Apply()
    {
        savedVolume = musicSlider.value;
        audioSource.volume = savedVolume;
        musicSlider.value = savedVolume;
        audioSetting.currentMusicVolume = savedVolume;

        sfxSavedVolume = sfxlider.value;
        sfxSource.volume = sfxSavedVolume;
        sfxlider.value = sfxSavedVolume;
        audioSetting.currentSFXVolume = sfxSavedVolume;
    }

    public void PlayOpenSound()
    {
        if (sfxSource != null)
        {
            if (openSound != null)
            {
                if (sfxSource.isPlaying)
                    sfxSource.Stop();
                sfxSource.clip = openSound;
                sfxSource.Play();
            }
        }
    }

    public void PlayCloseSound()
    {
        if (sfxSource != null)
        {
            if (closeSound != null)
            {
                if (sfxSource.isPlaying)
                    sfxSource.Stop();
                sfxSource.clip = closeSound;
                sfxSource.Play();
            }
        }
    }
    public void CloseHelpDesk()
    {
        helpDesk.SetActive(false);
    }
}

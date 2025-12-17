using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ControlSettingsControler : MonoBehaviour
{
    // ======= VOLUME SETTINGS =======
    // [SerializeField] private AudioMixer myMixer;
    // [SerializeField] private Slider musicSlider;
    // [SerializeField] private Slider SFXSlider;

    // ======= MIC SETTINGS =======
    [SerializeField] private Slider micSensitivitySlider;

    // Start is called before the first frame update
    void Start()
    {
        // if (PlayerPrefs.HasKey("musicVolume"))
        // {
        //     LoadVolume();
        // }
        // else
        // {
        //     SetMusicVolume();
        //     SetSFXVolume();
        // }

        if (PlayerManager.Instance != null)
        {
            micSensitivitySlider.value = (int)PlayerManager.Instance.GetLoudnessSensitivity();
        }
 
    }

    // public void SetSFXVolume()
    // {
    //     float volume = SFXSlider.value;
    //     myMixer.SetFloat("SFX", Mathf.Log(volume) * 20);
    //     PlayerPrefs.SetFloat("SFXVolume", volume);
    // }

    // public void SetMusicVolume()
    // {
    //     float volume = musicSlider.value;
    //     myMixer.SetFloat("music", Mathf.Log(volume) * 20);
    //     PlayerPrefs.SetFloat("musicVolume", volume);
    // }

    // public void LoadVolume()
    // {
    //     musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
    //     SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
    //     SetMusicVolume();
    // }
    
    public void OnSliderValueChanged(float value)
    {
        PlayerManager.Instance?.SetMicSensitivity(value);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Options : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;
    public AudioSource music;
    public AudioMixer sfx;

    private void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", music.volume);
        float sfxvolume = 0;
        sfx.GetFloat("SFX volume", out sfxvolume);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", Mathf.Pow(10, sfxvolume/20));
    }

    public void ChangeMusicVolume()
    {
        music.volume = musicSlider.value;
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
    }

    public void ChangeSFXVolume()
    {
        sfx.SetFloat("SFX volume", Mathf.Log10(sfxSlider.value) * 20);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
    }
}

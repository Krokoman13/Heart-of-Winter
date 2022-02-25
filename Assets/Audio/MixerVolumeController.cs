using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MixerVolumeController : MonoBehaviour
{
    [SerializeField] private AudioMixer musicMixer;
    [SerializeField] private AudioMixer fightmusicMixer;
    public void SetVolume (float sliderValue)
    {
        musicMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue)*20);
        fightmusicMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue)*20);


    }
}

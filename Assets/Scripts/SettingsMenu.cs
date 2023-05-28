using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// This script controlls the settings menu

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider masterVolumeSlider;

    private void Start()
    {
        // Sets the master slider to the current master volume value
        audioMixer.GetFloat("MasterVolume", out float currentVolume);
        masterVolumeSlider.value = currentVolume;
    }

    // Changes the volume to external float from volume slider
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private AudioSource soundEffectSource; // No need to name this "clickSoundEffect" or something. make it flexible and just change the audio from drag-and-drop.
    [SerializeField] private AudioMixer audioMixer; // You must make child groups, then open inspector to expose variables, to make it modifiable from scripts

    private void Start()
    {
        // Load player preferences for music and sound effects
        bool isMusicChecked = PlayerPrefs.GetInt("isMusicChecked", 1) == 1;
        bool isSoundEffectsChecked = PlayerPrefs.GetInt("isSoundEffectsChecked", 1) == 1;

        SetBackgroundMusic(isMusicChecked);
        SetSoundEffects(isSoundEffectsChecked);
    }

    public void SetBackgroundMusic(bool isEnabled)
    {
        backgroundMusicSource.mute = !isEnabled;
    }

    public void SetSoundEffects(bool isEnabled)
    {
        soundEffectSource.mute = !isEnabled;
    }

    public void PlaySoundEffect()
    {
        if (!soundEffectSource.mute)
        {
            soundEffectSource.Play(); // This is fine as long as you don't need to play multiple of the same audio at the same time.
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private AudioSource clickSoundEffectSource;
    [SerializeField] private AudioSource moveSoundEffectSource;
    [SerializeField] private AudioSource damageSoundEffectSource;
    [SerializeField] private AudioSource healSoundEffectSource;
    [SerializeField] private AudioSource buySellSoundEffectSource;
    [SerializeField] private AudioSource gameOverSoundEffectSource;
    [SerializeField] private AudioMixer audioMixer; // You must make child groups below the "Master" group (in the Audio Mixer ribbon, beside the project / console ribbons), then open inspector to expose variables, to make it modifiable from scripts. Also so that in the MusicController -> Inspector --> Audio Source Component, you can set the "Output" into one of those exposed parameters

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
        clickSoundEffectSource.mute = !isEnabled;
        moveSoundEffectSource.mute = !isEnabled;
        damageSoundEffectSource.mute = !isEnabled;
        buySellSoundEffectSource.mute = !isEnabled;
        gameOverSoundEffectSource.mute = !isEnabled;
    }

    public void PlayClickSoundEffect()
    {
        if (!clickSoundEffectSource.mute)
        {
            clickSoundEffectSource.Play(); // .Play()  is fine as long as you don't need to play multiple of the same audio at the same time. Because they will cut the unfinished audio early.
        }
    }

    public void PlayMoveSoundEffectSource()
    {
        if (!moveSoundEffectSource.mute)
        {
            moveSoundEffectSource.Play();
        }
    }

    public void PlayDamageSoundEffectSource()
    {
        if (!damageSoundEffectSource.mute)
        {
            damageSoundEffectSource.Play();
        }
    }

    public void PlayHealSoundEffectSource()
    {
        if (!healSoundEffectSource.mute)
        {
            healSoundEffectSource.Play();
        }
    }

    public void PlayBuySellSoundEffectSource()
    {
        if (!buySellSoundEffectSource.mute)
        {
            buySellSoundEffectSource.Play();
        }
    }

    public void PlayGameOverSoundEffectSource()
    {
        if (!gameOverSoundEffectSource.mute)
        {
            gameOverSoundEffectSource.Play();
        }
    }
}

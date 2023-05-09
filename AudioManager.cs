using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    public static AudioManager instance;

    public const string MASTER_KEY = "MasterVolume";
    public const string BACKGROUNDAUDIO_KEY = "BackgroundVolume";
    public const string SOUNDFX_KEY = "SoundFXVolume";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadVolume();

    }
    void LoadVolume() // volume is saved in VolumeSettings.cs
    {
        //load the saved keys and if it does not exist set it to 0f 
        VolumeSettings.masterVolume = PlayerPrefs.GetFloat(MASTER_KEY, 0f);
        VolumeSettings.backgroundAudioVolume = PlayerPrefs.GetFloat(BACKGROUNDAUDIO_KEY, 0f);
        VolumeSettings.soundFxVolume = PlayerPrefs.GetFloat(SOUNDFX_KEY, 0f);

        //Sets volumes from the loaded playerprefs above to mixer channels
        mixer.SetFloat(VolumeSettings.MIXER_MASTER, VolumeSettings.masterVolume);
        mixer.SetFloat(VolumeSettings.MIXER_BACKGROUNDAUDIO, VolumeSettings.backgroundAudioVolume);
        mixer.SetFloat(VolumeSettings.MIXER_SOUNDFX, VolumeSettings.soundFxVolume);
    }
}

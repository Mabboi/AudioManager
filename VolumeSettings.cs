using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;

    //Settings Buttons for pause menu
    [SerializeField] Button backButton;
    [SerializeField] Button settingButton;
    [SerializeField] Button vENCBackButton;

    //settings panel contents
    [SerializeField] GameObject settings;
    [SerializeField] GameObject main;

    //Two canvases of the Menu scene
    [SerializeField] GameObject settingsCanvas;
    [SerializeField] GameObject vENCMenuCanvas;

    //Mute and Unmute Buttons
    [SerializeField] Button muteButton;
    [SerializeField] Button unMuteButton;

    //masterChannel components
    [SerializeField] Button masterPlusButton;
    [SerializeField] Button masterMinusButton;
    [SerializeField] Slider masterSlider;

    //backgroundAudio components
    [SerializeField] Button backgroundAudioPlusButton;
    [SerializeField] Button backgroundAudioMinusButton;
    [SerializeField] Slider backgroundAudioSlider;


    //soundFx components
    [SerializeField] Button soundFxPlusButton;
    [SerializeField] Button soundFxMinusButton;
    [SerializeField] Slider soundFxSlider;

    //variables for the volumes, accessible anywhere
    [Range(0.0001f,1f)]
    public static float masterVolume;
    [Range(0.0001f, 1f)]
    public static float backgroundAudioVolume;
    [Range(0.0001f, 1f)]
    public static float soundFxVolume;

    //Volumelevels variables
    float masterVolumeLevel;
    float backgroundAudioVolumeLevel;
    float soundFxVolumeLevel;

    public const string MIXER_MASTER = "MasterVolume";
    public const string MIXER_BACKGROUNDAUDIO = "BackgroundVolume";
    public const string MIXER_SOUNDFX = "SoundFXVolume";

    private void Awake()
    {

        //button clicks for volume canvases
        settingButton.onClick.AddListener(LoadVolumeSettings);
        backButton.onClick.AddListener(BackToMainSettings);
        vENCBackButton.onClick.AddListener(BackToMenuLevelSettings);

        masterPlusButton.onClick.AddListener(MasterPlusVolume);
        masterMinusButton.onClick.AddListener(MasterMinusVolume);

        backgroundAudioPlusButton.onClick.AddListener(BackgroundAudioPlusVolume);
        backgroundAudioMinusButton.onClick.AddListener(BackgroundAudioMinusVolume);

        soundFxPlusButton.onClick.AddListener(soundFxPlusVolume);
        soundFxMinusButton.onClick.AddListener(soundFxMinusVolume);

        muteButton.onClick.AddListener(MuteVolume);
        unMuteButton.onClick.AddListener(UnMuteVolume);
    }
    private void Start()
    {
        // Get the Mixer master's channel value and saves it in masterVolumeLevel, if there is no key value, set it to 1f
        masterVolumeLevel = PlayerPrefs.GetFloat(MIXER_MASTER);
        mixer.SetFloat(MIXER_MASTER, masterVolumeLevel);// set the final masterVolumeLevel to the mixer channel

        //set back values of masterSlider 
        masterSlider.value = PlayerPrefs.GetFloat("MasterSlider", 1f);


        // Get the Mixer backgroundAudio's channel value and saves it in backgroundAudioVolumeLevel, if there is no key value, set it to 1f
        backgroundAudioVolumeLevel = PlayerPrefs.GetFloat(MIXER_BACKGROUNDAUDIO);
        mixer.SetFloat(MIXER_BACKGROUNDAUDIO, backgroundAudioVolumeLevel);

        //set back values of backgroundSlider
        backgroundAudioSlider.value = PlayerPrefs.GetFloat("BackgroundSlider", 1f);

        // Get the Mixer soundFx's channel value and save in soundFxVolumeLevel, if there is no key value, set it to 1f
        soundFxVolumeLevel = PlayerPrefs.GetFloat(MIXER_SOUNDFX);
        mixer.SetFloat(MIXER_SOUNDFX, soundFxVolumeLevel);

        //set back values of SoundFxSlider
        soundFxSlider.value = PlayerPrefs.GetFloat("SoundFxSlider", 1f);

        //if muted set mutebutton active else set unmute button active
        if (masterSlider.value == 0 && backgroundAudioSlider.value == 0 && soundFxSlider.value == 0)
        {
            unMuteButton.gameObject.SetActive(true);
            muteButton.gameObject.SetActive(false);
        }
        else 
        {
            unMuteButton.gameObject.SetActive(false);
            muteButton.gameObject.SetActive(true);
        }

        SetDefaultVolume();
    }

    //when the object with this script is disabled
    void OnDisable()
    {
        //save the volumes to the keys in AudioManager Script
        PlayerPrefs.SetFloat(AudioManager.MASTER_KEY, masterVolume);
        PlayerPrefs.SetFloat(AudioManager.BACKGROUNDAUDIO_KEY, backgroundAudioVolume);
        PlayerPrefs.SetFloat(AudioManager.SOUNDFX_KEY, soundFxVolume);
    }

    void MasterPlusVolume() 
    {
        //add 8f to MasterMixerChannel
        masterVolume += 8f;
        mixer.SetFloat(MIXER_MASTER, masterVolume);
        PlayerPrefs.SetFloat("MasterVol", masterVolume);

        //add 0.1 to MasterSlider value
        masterSlider.value += 0.1f;

        MasterChannelBoundaries();
        
        PlayerPrefs.SetFloat("MasterSlider", masterSlider.value);

        print("Master Volume: " + masterVolume);
    }
    void MasterMinusVolume()
    {
        //Subtract -8f from MasterMixerChannel
        masterVolume += -8f;
        mixer.SetFloat(MIXER_MASTER, masterVolume);
        PlayerPrefs.SetFloat("MasterVol", masterVolume);

        //Subtract -0.1 from MasterSlider value
        masterSlider.value += -0.1f;

        MasterChannelBoundaries();

        PlayerPrefs.SetFloat("MasterSlider", masterSlider.value);

        print("Master Volume: " + masterVolume);
    }
    void BackgroundAudioPlusVolume()
    {
        //add 8f to BackgroundMixerChannel
        backgroundAudioVolume += 8f;
        mixer.SetFloat(MIXER_BACKGROUNDAUDIO, backgroundAudioVolume);
        PlayerPrefs.SetFloat("BackgroundAudioVol", backgroundAudioVolume);

        //add 0.1 to backgroundAudioSlider value
        backgroundAudioSlider.value += 0.1f;

        BackgroundAudioChannelBoundaries();

        PlayerPrefs.SetFloat("BackgroundSlider", backgroundAudioSlider.value);

        print("BackgroundAudio Volume: " + backgroundAudioVolume);
    }
    void BackgroundAudioMinusVolume()
    {
        //Subtract -8f from BackgroundMixerChannel
        backgroundAudioVolume += -8f;
        mixer.SetFloat(MIXER_BACKGROUNDAUDIO, backgroundAudioVolume);
        PlayerPrefs.SetFloat("BackgroundAudioVol", backgroundAudioVolume);

        //subract -0.1 from backgroundAudioSlider value
        backgroundAudioSlider.value += -0.1f;

        BackgroundAudioChannelBoundaries();

        PlayerPrefs.SetFloat("BackgroundSlider", backgroundAudioSlider.value);

        print("BackgroundAudio Volume: " + backgroundAudioVolume);
    }
    void soundFxPlusVolume()
    {
        //add 8f to SoundFxMixerChannel
        soundFxVolume += 8f;
        mixer.SetFloat(MIXER_SOUNDFX, soundFxVolume);
        PlayerPrefs.SetFloat("soundFxVol", soundFxVolume);

        //add 0.1 to soundFxSlider value
        soundFxSlider.value += 0.1f;

        SoundFxChannelBoundaries();

        PlayerPrefs.SetFloat("SoundFxSlider", soundFxSlider.value);

        print("SoundFx Volume: " + soundFxVolume);
    }
    void soundFxMinusVolume()
    {
        //Subtract -8f from SoundFxMixerChannel
        soundFxVolume += -8f;

        mixer.SetFloat(MIXER_SOUNDFX, soundFxVolume);
        PlayerPrefs.SetFloat("SoundFxVol", soundFxVolume);

        //subtract -0.1f from soundFxSlider value
        soundFxSlider.value += -0.1f;

        SoundFxChannelBoundaries();

        PlayerPrefs.SetFloat("SoundFxSlider", soundFxSlider.value);

        print("SoundFx Volume: " + soundFxVolume);
    }

    void MuteVolume()
    {
        //Sets master volume from mixer to the lowest db "-80f"
        mixer.SetFloat(MIXER_MASTER, -80f);
        mixer.SetFloat(MIXER_BACKGROUNDAUDIO, -80f);
        mixer.SetFloat(MIXER_SOUNDFX, -80f);
        masterVolume = -80f;
        backgroundAudioVolume = -80f;
        soundFxVolume = -80f;
        masterSlider.value = 0;
        backgroundAudioSlider.value = 0;
        soundFxSlider.value = 0;
        PlayerPrefs.SetFloat("MasterSlider", masterSlider.value);
        PlayerPrefs.SetFloat("BackgroundSlider", backgroundAudioSlider.value);
        PlayerPrefs.SetFloat("SoundFxSlider", soundFxSlider.value);

        muteButton.gameObject.SetActive(false);
        unMuteButton.gameObject.SetActive(true);

        print("VOLUME IS MUTED!");
    }
        void UnMuteVolume()
    {
        //Sets master volume from mixer to "-40f"
        mixer.SetFloat(MIXER_MASTER, 0f);
        mixer.SetFloat(MIXER_BACKGROUNDAUDIO, 0f);
        mixer.SetFloat(MIXER_SOUNDFX, -40f);
        masterVolume = 0f;
        backgroundAudioVolume = -40f;
        soundFxVolume = -40f;
        masterSlider.value = 1f;
        backgroundAudioSlider.value = 1f;
        soundFxSlider.value = 0.500f;
        PlayerPrefs.SetFloat("MasterSlider", masterSlider.value);
        PlayerPrefs.SetFloat("BackgroundSlider", backgroundAudioSlider.value);
        PlayerPrefs.SetFloat("SoundFxSlider", soundFxSlider.value);

        muteButton.gameObject.SetActive(true);
        unMuteButton.gameObject.SetActive(false);

        print("VOLUME IS UNMUTED!");
    }
    void SetDefaultVolume()
    {
        if (masterVolume == -40f && backgroundAudioVolume == -40f && soundFxVolume == -40f)
        {
            //Sets master volume from mixer to  db "-40f"
            mixer.SetFloat(MIXER_MASTER, -40f);
            mixer.SetFloat(MIXER_BACKGROUNDAUDIO, -40f);
            mixer.SetFloat(MIXER_SOUNDFX, -40f);
            masterVolume = -40f;
            backgroundAudioVolume = -40f;
            soundFxVolume = -40f;
            masterSlider.value = 0.500f;
            backgroundAudioSlider.value = 0.500f;
            soundFxSlider.value = 0.500f;
            PlayerPrefs.SetFloat("MasterSlider", masterSlider.value);
            PlayerPrefs.SetFloat("BackgroundSlider", backgroundAudioSlider.value);
            PlayerPrefs.SetFloat("SoundFxSlider", soundFxSlider.value);

            muteButton.gameObject.SetActive(true);
            unMuteButton.gameObject.SetActive(false);
        }
    }

    void MasterChannelBoundaries()
    {
        //Set masterVolume channel to -80f if it exceed or is equals to -80f
        if (masterVolume <= -80f)
        {
            mixer.SetFloat(MIXER_MASTER, -80f);
            masterVolume = -80f;
        }
        // Set masterVolume channel to 0f if its greater than or is equal to 0f
        else if (masterVolume >= 0f)
        {
            mixer.SetFloat(MIXER_MASTER, 0f);
            masterVolume = 0f;
        }
    }
    void BackgroundAudioChannelBoundaries()
    {
        //Set BackgroundAudioVolume channel to -80f if it exceed or is equals to -80f
        if (backgroundAudioVolume <= -80f)
        {
            mixer.SetFloat(MIXER_BACKGROUNDAUDIO, -80f);
            backgroundAudioVolume = -80f;
        }
        // Set BackgroundAudioVolume channel to 0f if its greater than or is equal to 0f
        else if (backgroundAudioVolume >= 0f)
        {
            mixer.SetFloat(MIXER_BACKGROUNDAUDIO, 0f);
            backgroundAudioVolume = 0f;
        }
    }

    void SoundFxChannelBoundaries()
    {
        //Set SoundFxVolume channel to -81f if it exceed or is equals to -80f
        if (soundFxVolume <= -80f)
        {
            mixer.SetFloat(MIXER_SOUNDFX, -80f);
            soundFxVolume = -80f;
        }
        // Set SoundFxVolume channel to 0f if its greater than or is equal to 0
        else if (soundFxVolume >= 0f)
        {
            mixer.SetFloat(MIXER_SOUNDFX, 0f);
            soundFxVolume = 0f;
        }
    }
   void LoadVolumeSettings() 
    {
        settings.SetActive(true);
        main.SetActive(false);
        vENCBackButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(true);
    }
    void LoadMainMenu()
    {
        settings.SetActive(true);
        main.SetActive(false);
        vENCBackButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(true);
    }
    public void BackToMainSettings()
    {
        main.SetActive(true);
        settings.SetActive(false);
        vENCMenuCanvas.SetActive(false);
        backButton.gameObject.SetActive(false);
    }
    public void LoadVolSettings()
    {
        settingsCanvas.SetActive(true);
        vENCMenuCanvas.SetActive(false);

        settings.SetActive(true);
        main.SetActive(false);
        vENCBackButton.gameObject.SetActive(true);
    }
    public void BackToMenuLevelSettings()
    {
        vENCMenuCanvas.SetActive(true);
        settingsCanvas.SetActive(false);
    }
}

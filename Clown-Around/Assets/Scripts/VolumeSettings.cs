//Basado en tutorial AUDIO MIXERS In Unity de Kap Koder
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
   [SerializeField] AudioMixer mixer;
   [SerializeField] Slider sfxSlider;
   [SerializeField] Slider musicSlider;

   public const string MIXER_MUSIC = "volumenMusica";
   public const string MIXER_SFX = "volumenEfectos";

   void Awake()
    {
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY, 1f);
        sfxSlider.value = PlayerPrefs.GetFloat(AudioManager.SFX_KEY, 1f);
    }
    void OnDisable()
    {
        PlayerPrefs.SetFloat(AudioManager.MUSIC_KEY, musicSlider.value);
        PlayerPrefs.SetFloat(AudioManager.SFX_KEY, sfxSlider.value);
    }
    void SetMusicVolume(float value)
    {
        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value)*20);
    }

    void SetSFXVolume(float value)
    {
        mixer.SetFloat(MIXER_SFX, Mathf.Log10(value)*20);
    }
}

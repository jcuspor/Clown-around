//Basado en tutorial AUDIO MIXERS In Unity de Kap Koder
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    public static AudioManager instance;
    public const string MUSIC_KEY = "volumenMusica";
    public const string SFX_KEY = "volumenEfectos";

    void Awake()
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

    void LoadVolume() //Volume saved in VolumeSettings.cs
    {
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        float sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);
        mixer.SetFloat(VolumeSettings.MIXER_MUSIC, Mathf.Log10(musicVolume)*20);
        mixer.SetFloat(VolumeSettings.MIXER_SFX, Mathf.Log10(sfxVolume)*20);
    }
}

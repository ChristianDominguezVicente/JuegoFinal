using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;
    [SerializeField] private Toggle screen;
    [SerializeField] private TMP_Dropdown quality;

    private void Start()
    {
        if (PlayerPrefs.HasKey("VolumeMusic"))
            LoadMusic();
        else
            ChangeMusic();

        if (PlayerPrefs.HasKey("VolumeSFX"))
            LoadSFX();
        else
            ChangeSFX();

        if (PlayerPrefs.HasKey("FullScreen"))
            LoadScreen();
        else
            FullScreen();

        if (PlayerPrefs.HasKey("Quality"))
            LoadQuality();
        else
            ChangeQuality();
    }

    public void FullScreen()
    {
        Screen.fullScreen = screen.isOn;
        if (screen.isOn == false)
            PlayerPrefs.SetInt("FullScreen", 1);
        else
            PlayerPrefs.SetInt("FullScreen", 0);
    }

    public void ChangeMusic()
    {
        float volumen = musicSlider.value;
        audioMixer.SetFloat("VolumeMusic", Mathf.Log10(volumen) * 20);
        PlayerPrefs.SetFloat("VolumeMusic", volumen);
    }

    public void ChangeSFX()
    {
        float volumen = SFXSlider.value;
        audioMixer.SetFloat("VolumeSFX", Mathf.Log10(volumen) * 20);
        PlayerPrefs.SetFloat("VolumeSFX", volumen);
    }

    public void ChangeQuality()
    {
        int index = quality.value;
        PlayerPrefs.SetInt("Quality", index);
        QualitySettings.SetQualityLevel(index);
    }

    private void LoadMusic()
    {
        musicSlider.value = PlayerPrefs.GetFloat("VolumeMusic");
        ChangeMusic();
    }

    private void LoadSFX()
    {
        SFXSlider.value = PlayerPrefs.GetFloat("VolumeSFX");
        ChangeSFX();
    }

    private void LoadScreen()
    {
        if (PlayerPrefs.GetInt("FullScreen") == 1)
            screen.isOn = false;
        else
            screen.isOn = true;
        FullScreen();
    }

    private void LoadQuality()
    {
        quality.value = PlayerPrefs.GetInt("Quality");
        ChangeQuality();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Image soundsImage;
    [SerializeField] private Image vibrationImage;


    [Header(" Sounds ON OFF")]
    [SerializeField] private Sprite soundsOnSprite;
    [SerializeField] private Sprite soundsOffSprite;

    [Header(" Vibration ON OFF")]
    [SerializeField] private Sprite vibrationOnImage;
    [SerializeField] private Sprite vibrationOffImage;


    [Header(" Settings ")]
    private bool _soundsState;
    private bool _vibrationState;



    private void Start()
    {
        LoadStates();
    }

    public void SoundsOnButtonCallback()
    {  
        _soundsState = !_soundsState;
       UpdateSoundsState();
        SaveStates();
    }

    public void VibrationButtonCallback()
    {
       _vibrationState = !_vibrationState;
       UpdateVibrationState();
       SaveStates();
    }

    private void UpdateSoundsState()
    {
        if (_soundsState)
            EnableSounds();

        else DisableSounds();
    }

    private void UpdateVibrationState()
    {
        if(_vibrationState)
        EnableVibration();

        else DisableVibration();
    }

    private void EnableSounds()
    {
        VibrationManager.Vibrate();

        soundsImage.sprite = soundsOnSprite;
    }

    private void DisableSounds()
    {
        VibrationManager.Vibrate();

        soundsImage.sprite = soundsOffSprite;
    }

    private void EnableVibration()
    {
        VibrationManager.Vibrate();

        vibrationImage.sprite = vibrationOnImage;
        VibrationManager.Instance.EnableVibration();
    }

    private void DisableVibration()
    {
        vibrationImage.sprite = vibrationOffImage;
        VibrationManager.Instance.DisableHaptics();
    }

    private void LoadStates()
    {
        _soundsState = PlayerPrefs.GetInt("Sounds", 1) == 1;
        _vibrationState = PlayerPrefs.GetInt("Vibrations",1) == 1;

        UpdateSoundsState();
        UpdateVibrationState();
    }

    private void SaveStates()
    {
        PlayerPrefs.SetInt("Sounds",_soundsState ? 1 : 0);
        PlayerPrefs.SetInt("Vibrations", _vibrationState ? 1 : 0);
    }



}

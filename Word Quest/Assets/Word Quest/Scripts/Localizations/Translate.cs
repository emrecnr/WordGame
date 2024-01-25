using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class Translate : MonoBehaviour
{
    // en de es tr
    public int selectedLanguageIndex;

    private void Awake() 
    {
        //SaveLanguage();
        LoadLanguage();
    }
    public void OnSelect(int value)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[value];
        Debug.Log(LocalizationSettings.AvailableLocales.Locales[value]);
    }


    private void SaveLanguage()
    {
        PlayerPrefs.SetInt("Language", 3);
        PlayerPrefs.Save();
    }

    private void LoadLanguage()
    {
        selectedLanguageIndex =  PlayerPrefs.GetInt("Language",0);
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[selectedLanguageIndex];
        Debug.Log(selectedLanguageIndex);
    }
}

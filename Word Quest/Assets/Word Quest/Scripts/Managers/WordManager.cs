using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class WordManager : MonoBehaviour
{
    public static WordManager Instance {get; set;}

    [SerializeField] private LanguageData[] availableLanguages;

    [Header(" Elements ")]
    [SerializeField] private string secretWord;
    [SerializeField] private LanguageData languageData;
    private string words;

    [Header(" Settings ")]
    private bool _shouldReset;

    private void Awake() {
        if(Instance == null) Instance = this;

        else
        Destroy(gameObject);

        
        SetLanguageWordsAsset();


        // words = languageData.wordsText.text;
    }


    private void OnEnable() {
        GameManager.OnGameStateChanged += OnGameStateChangedHandler;
    }

    private void OnDisable() {
        GameManager.OnGameStateChanged -= OnGameStateChangedHandler;
    }
    private void Start() {
        SetNewSecretWord();
    }

    public void SetLanguageWordsAsset()
    {
        int languageIndex = PlayerPrefs.GetInt("Language");
        languageData = availableLanguages[languageIndex];
        Debug.Log(languageIndex);
    }

    private void OnGameStateChangedHandler(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Menu:

                break;

            case GameState.Game:
                    if(_shouldReset)
                            SetNewSecretWord();
                break;

            case GameState.LevelComplete:
                    _shouldReset = true;
                break;

            case GameState.Gameover:
                _shouldReset = true;
                break;
        }
    }

    public string GetSecretWord()
    {
        return secretWord.ToUpper();
    }

    public string GetWords()
    {
        List<string> wordList = new List<string>(languageData.wordsText.text.Split("\r\n"));
        for (int i = 0; i < wordList.Count; i++)
        {
            wordList[i] = wordList[i].ToUpperInvariant();
        }
        return string.Join(",", wordList);
    }
    

    private void SetNewSecretWord()
    {
       // Debug.Log("String Length : " + words.Length);
        // parse the text word list into a string list

        List<string> wordList = new List<string>(languageData.wordsText.text.Split("\r\n"));

        // pick a random index
        int wordIndex = Random.Range(0, wordList.Count);

        // set the new secret word
        secretWord = wordList[wordIndex].ToUpperInvariant(); // Turkish convert

        _shouldReset = false;
    }
}

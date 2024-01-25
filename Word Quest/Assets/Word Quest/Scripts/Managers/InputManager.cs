using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    [Header(" Elements ")]
    [SerializeField] private WordContainer[] wordContainers;
    [SerializeField] private Button tryButton;
    [SerializeField] private KeyboardColorizer keyboardColorizer;

    [Header(" Settings ")]
    private int _currentWordContainer;
    private bool canAddLetter = true;
    private bool _shouldReset;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        Initialize();
    }

    private void OnEnable()
    {
        KeyboardKey.OnKeyPressed += OnKeyPressedHandler;
        GameManager.OnGameStateChanged += OnGameStateChangedHandler;
    }

    private void OnDisable()
    {
        KeyboardKey.OnKeyPressed -= OnKeyPressedHandler;
        GameManager.OnGameStateChanged -= OnGameStateChangedHandler;
    }

    private void Initialize()
    {
        _currentWordContainer = 0;
        canAddLetter = true;

        DisableTryButton();

        for (int i = 0; i < wordContainers.Length; i++)
            wordContainers[i].Initialize();


        _shouldReset = false;
    }

    private void OnGameStateChangedHandler(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Game:
                if (_shouldReset)
                    Initialize(); // Reset the game
                break;

            case GameState.LevelComplete:
                _shouldReset = true;
                break;
            case GameState.Gameover:
                _shouldReset = true;
                break;
        }
    }


    private void OnKeyPressedHandler(char letter)
    {
        if (!canAddLetter) return;

        wordContainers[_currentWordContainer].Add(letter);

        if (wordContainers[_currentWordContainer].IsComplete())
        {
            canAddLetter = false;
            EnableTryButton();
        }
    }

    public void CheckWord()
    {
        string wordToCheck = wordContainers[_currentWordContainer].GetWord();

        // Burada wordToCheckin Kelimler.txt dosyasındaki herhangi bir kelimeyle eşleşip eşleşmediğini kontrol et
        string[] wordList = WordManager.Instance.GetWords().Split(',');

        if (Array.Exists(wordList, element => element == wordToCheck))
        {
            Debug.Log("Kelime Listede var");
            // wordToCheck, kelime listesinde bulunuyor.

            string secretWord = WordManager.Instance.GetSecretWord();

            wordContainers[_currentWordContainer].Colorize(secretWord); // check
            keyboardColorizer.Colorize(secretWord, wordToCheck);

            if (wordToCheck == secretWord)
            {
                Invoke("SetLevelComplete", 1f);
                DataManager.Instance.IncreaseWonCount();
            }
            else
            {
                _currentWordContainer++;
                DisableTryButton();

                if (_currentWordContainer >= wordContainers.Length)
                {
                    DataManager.Instance.ResetScore();
                    DataManager.Instance.IncreaseLoseCount();
                    GameManager.Instance.SwitchGameState(GameState.Gameover);
                }
                else
                {
                    canAddLetter = true;
                }
            }
        }
        else
        {
            Debug.Log("Kelime Listede Yok");
            wordContainers[_currentWordContainer].Colorize(null,true); // check
            bool removedWord = wordContainers[_currentWordContainer].RemoveWord();
            canAddLetter = true;
            if(removedWord) DisableTryButton();
        }

    }

    private void SetLevelComplete()
    {
        UpdateData();
        keyboardColorizer.ResetColorize();
        GameManager.Instance.SwitchGameState(GameState.LevelComplete);
        if(DataManager.Instance.RemoveAds == 0)
        {
            AdSource.Instance.GetAdProvider().ShowInterstitialAd();

        }
    }
    // Degısebilir
    private void UpdateData()
    {
        int scoreToAdd = 6 - _currentWordContainer;
        DataManager.Instance.IncreaseScore(scoreToAdd);
        DataManager.Instance.AddCoins(scoreToAdd * 2);
    }

    public void BackspacePressedCallback()
    {
        VibrationManager.Vibrate();

        if (GameManager.Instance.CurrentGameState == GameState.Gameover) return;
        bool removedLetter = wordContainers[_currentWordContainer].RemoveLetter();
        canAddLetter = true;
        if (removedLetter) DisableTryButton();

    }

    private void EnableTryButton()
    {
        tryButton.interactable = true;
    }
    private void DisableTryButton()
    {
        tryButton.interactable = false;
    }

    public WordContainer GetCurrentWordContainer()
    {
        return wordContainers[_currentWordContainer];
    }

}

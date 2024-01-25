using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KeyboardColorizer : MonoBehaviour
{

    [Header(" Elements ")]
    private KeyboardKey[] keys;

    [Header(" Settings ")]
    private bool _shouldReset;

    private void Awake() {
        keys = GetComponentsInChildren<KeyboardKey>();
    }

    private void OnEnable() {
        GameManager.OnGameStateChanged += OnGameStateChangedHandler;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= OnGameStateChangedHandler;
    }
    
    
    private void OnGameStateChangedHandler(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Game:
            if(_shouldReset)
                    Initialize();
                break;

            case GameState.LevelComplete:
                    _shouldReset = true;
                break;
            case GameState.Gameover:
                    _shouldReset = true;
                    break;
        }
    }

    private void Initialize()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            keys[i].Initialize();
        }
        _shouldReset = false;
    }


    public void Colorize(string secretWord, string wordToCheck)
    {
        for (int i = 0; i < keys.Length; i++)
        {
            char keyLetter = keys[i].GetLetter();

            for (int j = 0; j < wordToCheck.Length; j++)
            {
                if(keyLetter != wordToCheck[j]) continue;

                // the key letter we have pressed is equals to the current wordToCheck letter

                if(keyLetter == secretWord[j])
                {
                    // valid
                    keys[i].SetValid();
                }
                else if(secretWord.Contains(keyLetter))
                {
                    // potential
                    keys[i].SetPotential();
                }
                else{
                    // invalid
                    keys[i].SetInvalid();
                }
            }
        }
    }

    public void ResetColorize()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            keys[i].Initialize();
        }
    }
}

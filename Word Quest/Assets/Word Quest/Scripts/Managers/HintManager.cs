using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HintManager : MonoBehaviour
{

    [Header(" Elements ")]
    [SerializeField] private GameObject keyboard;
    private KeyboardKey[] keys;

    [Header(" Settings")]
    [SerializeField] TMP_Text hintKeyboardText;
    [SerializeField] TMP_Text hintLetterText;
    private bool _shouldReset;

    private void Awake()
    {
        keys = keyboard.GetComponentsInChildren<KeyboardKey>();

        Debug.Log("We found " + keys.Length);
    }
    
    private void Start() 
    {
       
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += OnGameStateChangedHandler;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= OnGameStateChangedHandler;
    }

    private void InitializeHint()
    {
        if (DataManager.Instance.HintKeyboardCount <= 0)
            hintKeyboardText.text = "Watch Ad";

        else
        {
            hintKeyboardText.text = DataManager.Instance.HintKeyboardCount.ToString();
        }

        if(DataManager.Instance.HintLetterCount <=0)
        {
            hintLetterText.text = "Watch Ad";
        }
        else
        {
            hintLetterText.text = DataManager.Instance.HintLetterCount.ToString();
        }
    }


    private void OnGameStateChangedHandler(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Menu:

                break;

            case GameState.Game:
                InitializeHint();
                if (_shouldReset)
                {
                    letterHintGivenIndices.Clear();
                    _shouldReset = false;
                }
                break;

            case GameState.LevelComplete:
                _shouldReset = true;
                break;

            case GameState.Gameover:
                _shouldReset = true;
                break;
        }
    }


    public void KeyboardHint()
    {
        if(DataManager.Instance.HintKeyboardCount > 0)
        {
            KeyboardHintReward();
            DataManager.Instance.HintKeyboardCount--;
        }
        else
        {
            AdSource.Instance.GetAdProvider().ShowRewardedAd(() =>
            {
                KeyboardHintReward();
                Debug.Log(" Ödül Verildi ");
                // 
            });
        }
        InitializeHint();
        DataManager.Instance.SaveData();
    }

    List<int> letterHintGivenIndices = new List<int>();
    public void LetterHint()
    {
        if (letterHintGivenIndices.Count >= 5) return;

        if (DataManager.Instance.HintLetterCount > 0)
        {
            LetterHintReward();
            DataManager.Instance.HintLetterCount--;
        }
        else
        {
            AdSource.Instance.GetAdProvider().ShowRewardedAd(() =>
            {
               LetterHintReward();
                Debug.Log(" Ödül Verildi ");
                // 
            });
        }
        InitializeHint();
        DataManager.Instance.SaveData();
    }

    private void LetterHintReward()
    {
        if (letterHintGivenIndices.Count >= 5) return;

        List<int> letterHintNotGivenIndices = new List<int>();

        for (int i = 0; i < 5; i++)
            if (!letterHintGivenIndices.Contains(i))
                letterHintNotGivenIndices.Add(i);


        WordContainer currentWordContainer = InputManager.Instance.GetCurrentWordContainer();

        string secretWord = WordManager.Instance.GetSecretWord();

        int randomIndex = letterHintNotGivenIndices[Random.Range(0, letterHintNotGivenIndices.Count)];
        letterHintGivenIndices.Add(randomIndex);

        currentWordContainer.AddAsHint(randomIndex, secretWord[randomIndex]);
    }

    private void KeyboardHintReward()
    {
        string secretWord = WordManager.Instance.GetSecretWord();

        List<KeyboardKey> untouchedKeys = new List<KeyboardKey>();

        for (int i = 0; i < keys.Length; i++)
        {
            if (keys[i].IsUntouched())
                untouchedKeys.Add(keys[i]);
        }

        // At this point, we have a list of all the untouched keys
        // Let is remove the ones that are in the secret word to avoid graying them out

        List<KeyboardKey> t_untouchedKeys = new List<KeyboardKey>(untouchedKeys);

        for (int i = 0; i < untouchedKeys.Count; i++)
        {
            if (secretWord.Contains(untouchedKeys[i].GetLetter()))
                t_untouchedKeys.Remove(untouchedKeys[i]);
        }

        // At this point, we have a list of all the untouched keys, not contained into the secret word
        if (t_untouchedKeys.Count <= 0) return;

        int randomKeyIndex = Random.Range(0, t_untouchedKeys.Count);
        t_untouchedKeys[randomKeyIndex].SetInvalid();
    }
}

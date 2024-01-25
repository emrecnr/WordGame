using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WordContainer : MonoBehaviour
{
    [Header(" Elements ")]
    private LetterContainer[] letterContainers;
    //[SerializeField]private SpriteRenderer[] letterContainerRenderers;

    [Header(" Settings ")]
    private int _currentLetterIndex;

    private void Awake() 
    {
        letterContainers = GetComponentsInChildren<LetterContainer>();
        //Initialize();
    }

    public void Initialize()
    {
        _currentLetterIndex = 0;
        
        for (int i = 0; i < letterContainers.Length; i++)
        {
            letterContainers[i].Initialize();
        }
    }

    public void Add(char letter)
    {
        letterContainers[_currentLetterIndex].SetLatter(letter);
        _currentLetterIndex++;
    }

    public void AddAsHint(int letterIndex, char letter)
    {
        letterContainers[letterIndex].SetLatter(letter, true);
    }

    public bool RemoveLetter()
    {
        if(_currentLetterIndex <=0) return false;

        _currentLetterIndex--;
        letterContainers[_currentLetterIndex].Initialize();

        return true;
    }

    public bool RemoveWord()
    {
        if(_currentLetterIndex <=0) return false;
        for (int i = 0; i < letterContainers.Length; i++)
        {
            _currentLetterIndex--;
            letterContainers[_currentLetterIndex].Initialize();
        }
        return true;

    }

    public void Colorize(string secretWord,bool isWord = false)
    {

        if (isWord)
        {
            for (int i = 0; i < letterContainers.Length; i++)
            {
                letterContainers[i].SetVoid();
            }
        }
        else
        {
            List<char> chars = new List<char>(secretWord.ToCharArray());


            for (int i = 0; i < letterContainers.Length; i++)
            {
                char letterToCheck = letterContainers[i].GetLetter();

                if (letterToCheck == secretWord[i])
                {
                    // Valid
                    letterContainers[i].SetValid();
                    chars.Remove(letterToCheck);
                }
                else if (chars.Contains(letterToCheck))
                {
                    // Potential
                    letterContainers[i].SetPotential();
                    chars.Remove(letterToCheck);
                }
                else
                {
                    // Invalid
                    letterContainers[i].SetInvalid();
                }
            }
        }        
    }


    public string GetWord()
    {
        string word = "";

        for (int i = 0; i < letterContainers.Length; i++)
        {
            word += letterContainers[i].GetLetter().ToString();
        }

        return word;
    }

    public bool IsComplete()
    {
        return _currentLetterIndex >= 5;
    }

}

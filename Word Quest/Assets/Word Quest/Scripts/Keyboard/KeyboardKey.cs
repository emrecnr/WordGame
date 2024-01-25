using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

enum Validity { None, Valid, Potential, Invalid}
public class KeyboardKey : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private TMP_Text letterText;
    [SerializeField] private Image image;

    [Header(" Settings ")]
    private Validity _validity;

    [Header(" Events ")]
    public static Action<char> OnKeyPressed;

    private void Start() 
    {
        GetComponent<Button>().onClick.AddListener(SendKeyPressedEvent);   
        Initialize();     
    }

    private void SendKeyPressedEvent()
    {
        VibrationManager.Vibrate();
        Debug.Log(letterText.text);
        OnKeyPressed?.Invoke(letterText.text[0]);
    }

    public char GetLetter()
    {
        return letterText.text[0];
    }

    public void Initialize()
    {
        image.color = Color.white;
        _validity = Validity.None;
    }

    public void SetValid()
    {
        image.color = Color.green;
        _validity = Validity.Valid;
    }

    public void SetPotential()
    {
        if(_validity == Validity.Valid) return;

        image.color = Color.yellow;
        _validity = Validity.Potential;
    }

    public void SetInvalid()
    {
        if (_validity == Validity.Valid || _validity == Validity.Potential) return;

        image.color = Color.grey;
        _validity = Validity.Invalid;
    }

    public bool IsUntouched()
    {
        return _validity == Validity.None;
    }

}

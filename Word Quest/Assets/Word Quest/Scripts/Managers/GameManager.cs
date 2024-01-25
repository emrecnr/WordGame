using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { Game, LevelComplete, Gameover, Idle, Shop, Profile, Settings, Menu}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header(" SETTINGS ")]
    private GameState _currentGameState;

    [Header(" EVENTS ")]
    public static Action<GameState> OnGameStateChanged;


    public GameState CurrentGameState =>_currentGameState;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(WaitForDataLoadedAndShowMenu());
    }

    public void SwitchGameState(GameState newState)
    {
        _currentGameState = newState;

        OnGameStateChanged?.Invoke(newState);
    }

    public void NextButtonCallback()
    {
        VibrationManager.Vibrate();

        SwitchGameState(GameState.Game);
    }

    public void PlayButtonCallback()
    {
        VibrationManager.Vibrate();

        SwitchGameState(GameState.Game);
    }

    public void BackButtonCallback()
    {
        VibrationManager.Vibrate();

        SwitchGameState(GameState.Menu);
    }

    private IEnumerator WaitForDataLoadedAndShowMenu()
    {
        yield return new WaitUntil(() => DataManager.Instance.IsDataLoaded);
        SwitchGameState(GameState.Menu);
    }
}

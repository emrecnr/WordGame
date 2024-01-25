using System;
using System.Collections;
using System.Collections.Generic;
using RDG;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header(" ELEMENTS ")]
    [SerializeField] private GameObject gameLetterObjects;
    [SerializeField] private GameObject gameKeyboardUI;
    [SerializeField] private CanvasGroup menuCG;
    [SerializeField] private CanvasGroup gameCG;
    [SerializeField] private CanvasGroup levelCompleteCG;
    [SerializeField] private CanvasGroup gameoverCG;
    [SerializeField] private CanvasGroup settingsCG;
    [SerializeField] private CanvasGroup profileCG;
    [SerializeField] private CanvasGroup shopCG;
    [SerializeField] private CanvasGroup bgCg;

    [Header(" Menu Elements ")]
    [SerializeField] private Image bgImage;
    [SerializeField] private TMP_Text menuCoins;
    [SerializeField] private TMP_Text menuLetter;
    [SerializeField] private TMP_Text menuKeyboard;
    [SerializeField] private TMP_Text profileCoins;
    [SerializeField] private Button signOutButton;
    // maybe best score

    [Header(" Level Complete Elements ")]

    #region Texts
    [SerializeField] private TMP_Text levelCompleteCoins, levelCompleteSecretWord, levelCompleteScore, levelCompleteBestScore, nicknameText;
    #endregion

    [Header(" Game Elements ")]
    [SerializeField] private TMP_Text gameScore;
    [SerializeField] private TMP_Text gameCoins;

    [Header(" Gameover Elements ")]
    [SerializeField] private TMP_Text gameoverSecretWord;
    [SerializeField] private TMP_Text gameoverBestScore;

    [Header(" Settings Elements ")]
    [SerializeField] private TMP_Text settingsCoin;

    [Header(" Won Lose ")]
    [SerializeField] private TMP_Text wonCount;
    [SerializeField] private TMP_Text loseCount;

    private void Awake()
    {
        if (Instance == null)
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
        ShowMenu();
        HideGame();
        HideLevelComplete();
        HideGameOver();
        HideSettings();
        HideProfile();
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += GameStateChangedHandler;
    }
    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= GameStateChangedHandler;
    }

    private void GameStateChangedHandler(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Menu:
                ShowBG();
                ShowMenu();
                HideLevelComplete();
                HideGame();
                HideGameOver();
                HideSettings();
                HideProfile();
                HideShop();
                break;

            case GameState.Game:                
                HideCG();
                HideMenu();
                HideLevelComplete();
                HideGameOver();
                ShowGame();
                break;

            case GameState.LevelComplete:
                ShowLevelComplete();
                HideGame();
                break;

            case GameState.Gameover:
                ShowGameover();
                HideGame();
                break;
            case GameState.Profile:
                HideMenu();
                break;
            case GameState.Settings:
                HideMenu();
                break;
            case GameState.Shop:
                HideMenu();
                break;
        }
    }

    private void ShowBG()
    {
        bgCg.alpha = 1;
    }
    private void HideCG()
    {        
        bgCg.alpha = 0;
    }

    private void ShowMenu()
    {
        menuCoins.text = DataManager.Instance.Coin.ToString();
        menuKeyboard.text = DataManager.Instance.HintKeyboardCount.ToString();
        menuLetter.text = DataManager.Instance.HintLetterCount.ToString();
        ShowCG(menuCG);
    }

    private void HideMenu()
    {
        HideCG(menuCG);
    }

    private void ShowGame()
    {
        gameLetterObjects.SetActive(true);
        gameKeyboardUI.SetActive(true);
        gameCoins.text = DataManager.Instance.Coin.ToString();
        gameScore.text = DataManager.Instance.Score.ToString();
        ShowCG(gameCG);
    }

    private void HideGame()
    {
        gameKeyboardUI.SetActive(false);
        gameLetterObjects.SetActive(false);
        HideCG(gameCG);
    }

    private void ShowGameover()
    {
        gameoverSecretWord.text = WordManager.Instance.GetSecretWord();
        gameoverBestScore.text = DataManager.Instance.BestScore.ToString();

        ShowCG(gameoverCG);
    }

    private void HideGameOver()
    {
        HideCG(gameoverCG);
    }

    private void ShowLevelComplete()
    {
        levelCompleteCoins.text = DataManager.Instance.Coin.ToString();
        levelCompleteSecretWord.text = WordManager.Instance.GetSecretWord();
        levelCompleteScore.text = DataManager.Instance.Score.ToString();
        levelCompleteBestScore.text = DataManager.Instance.BestScore.ToString();

        ShowCG(levelCompleteCG);
    }

    private void HideLevelComplete()
    {
        HideCG(levelCompleteCG);
    }

    public void ShowSettings()
    {
        settingsCoin.text = DataManager.Instance.Coin.ToString();
        Vibrate();
        signOutButton.onClick.AddListener(()=>
        {
            LoginManager.Instance.Logout();
            SceneManager.LoadScene(0);
        });
        GameManager.Instance.SwitchGameState(GameState.Settings);
        ShowCG(settingsCG);
    }
    private void HideSettings()
    {
        HideCG(settingsCG);
    }

    public void ShowShop()
    {
        Vibrate();
        GameManager.Instance.SwitchGameState(GameState.Shop);
        ShowCG(shopCG);
    }

    private void HideShop()
    {
        HideCG(shopCG);
    }

    public void ShowProfile()
    {
        Vibrate();
        GameManager.Instance.SwitchGameState(GameState.Profile);
        profileCoins.text = DataManager.Instance.Coin.ToString();
        wonCount.text = DataManager.Instance.WonCount.ToString();
        loseCount.text = DataManager.Instance.LoseCount.ToString();
        nicknameText.text = DataManager.Instance.Nickname;
        ShowCG(profileCG);
    }

    private void HideProfile()
    {
        HideCG(profileCG);
    }

    private void ShowCG(CanvasGroup cg , CanvasGroup hideCg = null)
    {
        cg.gameObject.transform.localPosition = new Vector2(-Screen.width, 0);
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
        cg.gameObject.transform.LeanMoveLocalX(0, .25f).setEaseOutExpo();
    }

    private void HideCG(CanvasGroup cg)
    {
        cg.gameObject.transform.LeanMoveLocalX(Screen.width, .25f).setEaseOutExpo().setOnComplete(() =>
        {
            Debug.Log("Animasyon tamamlandı!");
            cg.alpha = 0;
            cg.interactable = false;
            cg.blocksRaycasts = false;
        });
    }

    private void Vibrate()
    {
        Vibration.Vibrate(100, 50);
    }
}

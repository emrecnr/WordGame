using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    [Header(" Data ")]
    private int _coins;
    private int _score = 0;
    private int _bestScore = 0;
    private string _nickname;
    private int _wonCount = 0;
    private int _loseCount = 0;
    private bool _isDataLoaded;
    private int _hintKeyboardCount;
    private int _hintLetterCount;
    private int _removeAds;


    public int Coin => _coins;
    public int Score => _score;
    public int BestScore => _bestScore;
    public string Nickname => _nickname;
    public int WonCount => _wonCount;
    public int LoseCount => _loseCount;
    public bool IsDataLoaded => _isDataLoaded;
    public int HintKeyboardCount {get{return _hintKeyboardCount;} set {_hintKeyboardCount = value;}}
    public int HintLetterCount {get{return _hintLetterCount;} set {_hintLetterCount = value;}}
    public int RemoveAds { get { return _removeAds; } set { _removeAds = value; } }



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    public void AddCoins(int amount)
    {
        _coins += amount;
        SaveData();
    }

    public void RemoveCoins(int amount)
    {
        _coins -= amount;
        _coins = Mathf.Max(_coins,0);
        SaveData();
    }

    public void IncreaseScore(int amount)
    {
        _score += amount;

        if(_score > _bestScore) _bestScore = _score;
        SaveData();
    }

    public void ResetScore()
    {
        _score = 0;
        SaveData();
    }

    public void IncreaseWonCount()
    {
        _wonCount ++;
        SaveData();
    }

    public void IncreaseLoseCount()
    {
        _loseCount ++;
        SaveData();
    }

    public void IncreaseHintKeyboardCount()
    {
        HintKeyboardCount++;
        SaveData();
    }

    public void IncreaseHintWordCount()
    {
        HintLetterCount++;
        SaveData();
    }

    public void RemoveHintWordCount()
    {
        HintLetterCount--;
        SaveData();
    }

    public void RemoveHintKeyboardCount()
    {
        HintKeyboardCount--;
        SaveData();
    }

    public void SetRemoveAds()
    {
        _removeAds = 1;
        SaveData();
    }

    public void SetNoRemoveAds()
    {
        _removeAds = 0;
        SaveData();
    }

    public void SaveData()
    {
        DatabaseManager.Instance.SaveData(new UserData{UserID = LoginManager.Instance.GetUserId(), Coin = _coins,Score = _score, Nickname = _nickname,WonCount = _wonCount, LoseCount = _loseCount,HintKeyboardCount = _hintKeyboardCount,HintLetterCount = _hintLetterCount, RemoveAds = _removeAds});
    }

    public void LoadData()
    {
        DatabaseManager.Instance.LoadData(LoginManager.Instance.GetUserId(), (userData) =>{            
            _coins = userData.Coin;
            _bestScore = userData.Score;
            _wonCount = userData.WonCount;
            _loseCount = userData.LoseCount;
            _nickname = userData.Nickname;
            _hintKeyboardCount = userData.HintKeyboardCount;
            _hintLetterCount = userData.HintLetterCount;
            _removeAds = userData.RemoveAds;

            _isDataLoaded = true;
        });   
    }
}

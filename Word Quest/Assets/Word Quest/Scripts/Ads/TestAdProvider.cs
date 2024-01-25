using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAdProvider : MonoBehaviour, AdProvider
{

    private string _rewardedAdID;
    private void Start() {
        Initialize();
    }
    public void Initialize()
    {
        //
    }

    public bool IsRewardedAdReady()
    {
        return true;
    }

    public void ShowRewarded(Action onRewardGranted, Action onQuited)
    {
        Debug.Log(" Test RewardedAd Completed ");
        onRewardGranted?.Invoke();
    }

    public string Vendor()
    {
        return " Test Ad Provider ";
    }

    public string GetAdID()
    {
        throw new NotImplementedException();
    }

    public void ShowRewardedAd(Action onRewarGranted)
    {
        throw new NotImplementedException();
    }

    public void ShowInterstitialAd()
    {
        throw new NotImplementedException();
    }
}

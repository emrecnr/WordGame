using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface AdProvider
{
    public void ShowRewardedAd(Action onRewarGranted);
    public void ShowInterstitialAd();
    public void Initialize();
    public string Vendor();
    }

using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using Unity.VisualScripting;
using UnityEngine;

public class AdmobProvider : AdProvider
{
    private string _rewardedAdID = "ca-app-pub-6788314039807028/2369457512"; // Test
    private string _bannerAdID = "ca-app-pub-6788314039807028/5778460735"; // Test
    private string _interstitialAdId= "ca-app-pub-6788314039807028/7234124588";

    private RewardedAd _rewardedAd;
    private BannerView _bannerAd;
    private InterstitialAd _interstitialAd;

    public AdmobProvider()
    {
        Initialize();
        LoadRewardedAd();
        if(PlayerPrefs.GetInt("RemoveAds") == 0)
        {
            LoadBannerAd();
            LoadInterstitialAd();
        }                
    }

    public void LoadInterstitialAd()
    {
        if(_interstitialAd!= null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");
        InterstitialAd.Load(_interstitialAdId, adRequest,(InterstitialAd ad,LoadAdError error) =>{
            if(error != null || ad==null)
            {
                Debug.Log(error);
                return;
            }
            _interstitialAd = ad;
            InterstitialEvent(_interstitialAd);
        });
    }

    public void ShowInterstitialAd()
    {
        if(_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            _interstitialAd.Show();
        }
        else
        {
            Debug.Log("Intersititial ad not ready");
        }
    }

    public void InterstitialEvent(InterstitialAd ad)
    {
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial Ad full screen content closed.");

            // Reload the ad so that we can show another as soon as possible.
            LoadInterstitialAd();
        };
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
        Debug.LogError("Interstitial ad failed to open full screen content " +
                       "with error : " + error);

        // Reload the ad so that we can show another as soon as possible.
        LoadInterstitialAd();
        };
    }

    public void CreateBannerView()
    {
        if (_bannerAd != null)
        {
            _bannerAd.Destroy();
            _bannerAd = null;
        }
        AdSize Adsize = new(320,50);
            _bannerAd = new BannerView(_bannerAdID, Adsize, AdPosition.Bottom);
    }

    public void LoadBannerAd()
    {
        if (_bannerAd == null)
        {
            CreateBannerView();
        }

        var adRequest = new AdRequest();

        Debug.Log("Loading banner ad.");
        _bannerAd.LoadAd(adRequest);
    }

    private void LoadRewardedAd()
    {
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }
        Debug.Log("Loading the rewarded ad.");
        var adRequest = new AdRequest();

        RewardedAd.Load(_rewardedAdID, adRequest,
          (RewardedAd ad, LoadAdError error) =>
          {
              // if error is not null, the load request failed.
              if (error != null || ad == null)
              {
                  Debug.LogError("Rewarded ad failed to load an ad " +
                                 "with error : " + error);
                  return;
              }

              Debug.Log("Rewarded ad loaded with response : "
                        + ad.GetResponseInfo());

              _rewardedAd = ad;
              RegisterEventHandlers(_rewardedAd);
          });
    }    

    public void Initialize()
    {
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
         //
        });
    }

    public void ShowRewardedAd(Action onRewardGranted)
    {
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) =>
            {
                // Reward the user.
                onRewardGranted?.Invoke();
            });
        }
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        ad.OnAdFullScreenContentClosed += () =>
        {
            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAd();
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAd();
        };
    }

    public string Vendor()
    {
        return null;
    }
}

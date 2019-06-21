using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdManager : MonoBehaviour
{
    public static AdManager instance;

    private string appID = "ca-app-pub-3225802372041416~6427479925";

    private BannerView bannerView;
    private string bannerID = "";

    private InterstitialAd fullScreenAd;
    private string fullScreenAdID = "";

    private RewardedAd rewardedAdMoney;
    private RewardedAd rewardedAdBlackShip;
    private RewardedAd rewardedAdBonus;
    private string rewardedAdIDMoney = "";
    private string rewardedAdIDBlackShip = "";
    private string rewardedAdIDBonus = "";

    private void Awake()
    {
        MobileAds.Initialize(appID);

        if (instance = null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
#if UNITY_ANDROID
        rewardedAdIDMoney = "ca-app-pub-3940256099942544/5224354917";
        rewardedAdIDBlackShip = "ca-app-pub-3940256099942544/5224354917";
        rewardedAdIDBonus = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
            rewardedAdIDMoney = "ca-app-pub-3940256099942544/1712485313";
            rewardedAdIDBlackShip = "ca-app-pub-3940256099942544/1712485313";
            rewardedAdIDBonus = "ca-app-pub-3940256099942544/1712485313";
#else
            rewardedAdIDMoney = "unexpected_platform";
            rewardedAdIDBlackShip = "unexpected_platform";
            rewardedAdIDBonus = "unexpected_platform";
#endif

        RequestFullScreenAd();

        rewardedAdMoney = new RewardedAd(rewardedAdIDMoney);
        rewardedAdBlackShip = new RewardedAd(rewardedAdIDBlackShip);
        rewardedAdBonus = new RewardedAd(rewardedAdIDBonus);

        RequestRewardedAdMoney();
        RequestRewardedAdBlackShip();
        RequestRewardedAdBonus();
    }

    public void RequestBunner()
    {
        bannerView = new BannerView(bannerID, AdSize.Banner, AdPosition.Bottom);
        AdRequest request = new AdRequest.Builder().Build();
        bannerView.LoadAd(request);
        bannerView.Show();
    }

    public void HideBanner()
    {
        bannerView.Hide();
    }

    public void RequestFullScreenAd()
    {
        fullScreenAd = new InterstitialAd(fullScreenAdID);
        AdRequest request = new AdRequest.Builder().Build();
        fullScreenAd.LoadAd(request);
    }

    public void ShowFullScreenAd()
    {
        if (fullScreenAd.IsLoaded())
        {
            fullScreenAd.Show();
        }
        else
        {
            Debug.Log("Full screen Ad not loaded");
        }
    }

    public void RequestRewardedAdMoney()
    {
        AdRequest request = new AdRequest.Builder().Build();
        rewardedAdMoney.LoadAd(request);
    }

    public void RequestRewardedAdBlackShip()
    {
        AdRequest request = new AdRequest.Builder().Build();
        rewardedAdBlackShip.LoadAd(request);
    }

    public void RequestRewardedAdBonus()
    {
        AdRequest request = new AdRequest.Builder().Build();
        rewardedAdBonus.LoadAd(request);
    }

    public void ShowRewardedAdMoney()
    {
        if (rewardedAdMoney.IsLoaded())
        {
            rewardedAdMoney.Show();
        }
        else
        {
            Debug.Log("Rewarded Ad not loaded");
        }
    }

    public void ShowRewardedAdBlackShip()
    {
        if (rewardedAdBlackShip.IsLoaded())
        {
            rewardedAdBlackShip.Show();
        }
        else
        {
            Debug.Log("Rewarded Ad not loaded");
        }
    }

    public void ShowRewardedAdBonus()
    {
        if (rewardedAdBonus.IsLoaded())
        {
            rewardedAdBonus.Show();
        }
        else
        {
            Debug.Log("Rewarded Ad not loaded");
        }
    }
}

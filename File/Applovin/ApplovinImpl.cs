using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ApplovinImpl : Singleton<ApplovinImpl>
{
    private Reward reward = new Reward();
    private Interstitial interstitial = new Interstitial();

    public Action<bool> rewardAction = null;
    public Action interstitialAction = null;

    public void initMax()
    {
        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
        {
            // AppLovin SDK is initialized, start loading ads
            Debug.Log("applovin sdk 初始化完成，开始加载广告...");
            loadRewardAd();
            // loadInterstitialAd();
        };
#if !UNITY_EDITOR && UNITY_ANDROID
            Debug.Log("applovin android 初始化");
            MaxSdk.SetSdkKey(Params.applovin_sdk_key_ios);
#elif !UNITY_EDITOR && UNITY_IOS
        Debug.Log("applovin ios 初始化");
        MaxSdk.SetSdkKey(Params.applovin_sdk_key_ios);
#else
        Debug.Log("applovin unity 初始化");
#endif
        // MaxSdk.SetUserId("USER_ID");
        MaxSdk.InitializeSdk();
    }

    public void loadRewardAd()
    {
        Debug.Log("开始加载激励视频...reward=" + reward);
        reward.initRewardListener();
        Debug.Log("加载激励视频完毕，等待回调");
    }

    public void loadInterstitialAd()
    {
        interstitial.initInterstitial();
    }

    public void showReward(Action<bool> action, string ActionName)
    {
        this.rewardAction = action;
        reward.showReward(ActionName);
    }

    public void showInterstitial(Action action)
    {
        this.interstitialAction = action;
        interstitial.showInterstitial();
    }

    



    

}

using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Reward
{

    private int retryAttempt;
    private bool isReady = false;
    private int clickCount; //视频点击次数

    public async void initRewardListener()
    {
        // Attach callback
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
        loadAd(0); //立即开始加载
    }

    public async void loadAd(int delayTime)
    {
        initParams();
        await System.Threading.Tasks.Task.Delay(delayTime);
#if !UNITY_EDITOR && UNITY_ANDROID
            MaxSdk.LoadRewardedAd(Params.reward_id_android);
#elif !UNITY_EDITOR && UNITY_IOS
            Debug.Log("ios-加载激励视频");
            MaxSdk.LoadRewardedAd(Params.reward_id_ios);
#else
        Debug.Log("unity-加载激励视频");
#endif
    }

    //在视频加载前，需要把一些参数置为初始状态
    public void initParams()
    {
        clickCount = 0;
        Params.reward_action_name = "";
    }

    //     public async void loadAd(int retryDelay)
    //     {
    //         await System.Threading.Tasks.Task.Delay(retryDelay);

    // #if !UNITY_EDITOR && UNITY_ANDROID
    //             MaxSdk.LoadRewardedAd(Params.reward_id_android);
    // #elif !UNITY_EDITOR && UNITY_IOS
    //             Debug.Log("ios-加载激励视频");
    //             MaxSdk.LoadRewardedAd(Params.reward_id_ios);
    // #else
    //         Debug.Log("unity-加载激励视频");
    // #endif
    //     }

    public async void delayLogEvent()
    {
        await Task.Run(() =>
        {
            //这个打点不用在这里发送，由游戏逻辑发送
            ADLogManager.Instance.rewardShowEvent("BP_RewardShow", Params.reward_action_name);
            Task.Delay(300).Wait();
        });
    }

    public void showReward(string ActionName)
    {
        if (isReady)
        {
            if (null != ActionName && "" != ActionName)
            {
                Params.reward_action_name = ActionName;
            }
#if !UNITY_EDITOR && UNITY_ANDROID
            delayLogEvent();
            MaxSdk.ShowRewardedAd(Params.reward_id_android);
#elif !UNITY_EDITOR && UNITY_IOS
            Debug.Log("ios-播放激励视频");
            delayLogEvent();
            MaxSdk.ShowRewardedAd(Params.reward_id_ios);
#else
            Debug.Log("unity-播放激励视频");
#endif
        }
        else
        {
            Debug.Log("激励视频还未加载完成");
            ApplovinImpl.Instance.rewardAction.Invoke(false);
        }
    }

    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        //load successed
        isReady = true;
        retryAttempt = 0;
    }

    private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        //load failed
        Debug.Log("激励视频加载失败,errorInfo=" + errorInfo.ToString() + ",retryAttempt=" + retryAttempt);
        isReady = false;
        retryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, retryAttempt));
        loadAd((retryAttempt * 1000));

    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("激励视频显示成功");
    }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad failed to display. AppLovin recommends that you load the next ad.
        // LoadRewardedAd();
        Debug.Log("激励视频显示失败,重新加载,ERROR=" + errorInfo.ToString() + ",adInfo=" + adInfo.ToString() + ",retryAttempt=" + retryAttempt);
        ApplovinImpl.Instance.rewardAction.Invoke(false);
        loadAd(0);
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("激励视频点击");
        clickCount++;

    }

    private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is hidden. Pre-load the next ad
        Debug.Log("激励视频关闭，加载下一个广告");
        for (int i = 0; i < clickCount; i++)
        {
            ADLogManager.Instance.clickEvent("BP_RewardLost", Params.reward_action_name);
        }
        loadAd(0);
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        // The rewarded ad displayed and the user should receive the reward.
        Debug.Log("发放激励视频奖励");
        isReady = false;
        ApplovinImpl.Instance.rewardAction.Invoke(true);
    }

    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Ad revenue paid. Use this callback to track user revenue.
        Debug.Log("激励视频收入,adInfo=" + adInfo.Revenue);
        ADLogManager.Instance.revenueEvent("AppLovin", adInfo.NetworkName, adInfo.AdUnitIdentifier, adInfo.AdFormat, adInfo.Revenue);
    }


}

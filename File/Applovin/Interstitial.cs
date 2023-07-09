using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Interstitial
{

    private int retryAttempt;

    private bool isReady = false;

    public void initInterstitial()
    {
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnInterstitialRevenuePaidEvent;
        loadInterstitial(retryAttempt);
    }

    private async void loadInterstitial(float retryDelay)
    {
#if !UNITY_EDITOR && UNITY_ANDROID
            await Task.Run(() => {
            Task.Delay((int)(retryDelay * 1000)).Wait();
            MaxSdk.LoadInterstitial(Params.interstitial_id_android);
        });
#elif !UNITY_EDITOR && UNITY_IOS
                await Task.Run(() => {
                Task.Delay((int)(retryDelay * 1000)).Wait();
                Debug.Log("ios-加载插屏广告");
                MaxSdk.LoadInterstitial(Params.interstitial_id_ios);
            });
#else
        await Task.Run(() =>
        {
            Task.Delay((int)(retryDelay * 1000)).Wait();
            Debug.Log("unity-加载插屏广告");
        });
#endif

    }

    public async void delayLogEvent()
    {
        await Task.Run(() =>
        {
            // SocialManager.Instance.LogEvent("InterShow", "");
            Task.Delay(300).Wait();
        });
    }

    public void showInterstitial()
    {
        if (isReady)
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            delayLogEvent();
            MaxSdk.ShowInterstitial(Params.interstitial_id_android);
#elif !UNITY_EDITOR && UNITY_IOS
            Debug.Log("ios-播放插屏广告");
            delayLogEvent();
            MaxSdk.ShowInterstitial(Params.interstitial_id_ios);
#else
            Debug.Log("unity-播放插屏广告");
#endif

        }
        else
        {
            Debug.Log("插屏广告还未加载完成");
            ApplovinImpl.Instance.interstitialAction.Invoke();
        }
    }

    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is ready for you to show. MaxSdk.IsInterstitialReady(adUnitId) now returns 'true'

        // Reset retry attempt
        Debug.Log("插屏广告加载成功");
        retryAttempt = 0;
    }

    private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Interstitial ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds)
        Debug.Log("插屏广告加载失败,重新加载");
        retryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, retryAttempt));
        loadInterstitial((float)retryDelay);
    }

    private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("插屏广告显示成功");
    }

    private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad failed to display. AppLovin recommends that you load the next ad.
        Debug.Log("插屏广告显示失败,重新加载");
        ApplovinImpl.Instance.interstitialAction.Invoke();
        loadInterstitial(retryAttempt);
    }

    private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("插屏广告点击");
    }

    private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is hidden. Pre-load the next ad.
        Debug.Log("插屏广告关闭,重新加载");
        loadInterstitial(retryAttempt);
    }

    private void OnInterstitialRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo impressionData)
    {
        // Interstitial ad revenue has been paid out. You can access the revenue amount and currency from the 'impressionData' object.
        Debug.Log("插屏广告收益");
        SDKManager.Instance.logAdRevenue("AppLovin", impressionData.NetworkName, impressionData.AdUnitIdentifier, impressionData.AdFormat, impressionData.Revenue);
    }


}

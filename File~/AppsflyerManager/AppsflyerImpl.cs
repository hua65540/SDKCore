using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppsFlyerSDK;
#if !UNITY_ANDROID && !UNITY_EDITOR
using Unity.Advertisement.IosSupport;
using Unity.Notifications.iOS;
#endif
using System.Text;
using Castle.Core.Smtp;

public class AppsflyerImpl : MonoBehaviour, IAppsFlyerConversionData
{

    void Awake()
    {
        Debug.Log("appsflyer init");
        DontDestroyOnLoad(this);
        init();
    }

    void Start()
    {

    }

    public void init()
    {
        AppsFlyer.OnRequestResponse += (sender, args) =>
        {
            var af_args = args as AppsFlyerRequestEventArgs;
            Debug.Log("appsflyer初始化状态: status code=" + af_args.statusCode);
        };
        AppsFlyer.setIsDebug(Params.af_isDebug); //开启调试模式
        string user_id = PlayerPrefs.GetString(Params.key_user_id, "");
        if (!user_id.Equals(""))
        {
            Debug.Log("设置自定义user_id");
            AppsFlyer.setCustomerUserId(user_id);
        }
#if !UNITY_EDITOR && UNITY_ANDROID
        Debug.Log("af_android开始初始化");
        AppsFlyer.initSDK(Params.af_sdk_key_android, "",this);
        AppsFlyer.startSDK();
#elif !UNITY_EDITOR && UNITY_IOS
        Debug.Log("等待用户对idfa收集进行授权...");
        AppsFlyer.waitForATTUserAuthorizationWithTimeoutInterval(60);
        Debug.Log("af_ios开始初始化,appid=" + Params.af_app_id_ios);
        AppsFlyer.initSDK(Params.af_sdk_key_ios, Params.af_app_id_ios, this);
        AppsFlyer.startSDK();
        requesPermission(); //请求权限，只要有回调就会请求权限
#endif


    }

    public void requesPermission()
    {
#if !UNITY_ANDROID && !UNITY_EDITOR
        Debug.Log("开始请求权限...");
        if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
        {
            ATTrackingStatusBinding.RequestAuthorizationTrackingCompleteHandler attCallback = RequestAuthorizationTrackingCompleteHandler;
            ATTrackingStatusBinding.RequestAuthorizationTracking(attCallback);
        }
        else if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == ATTrackingStatusBinding.AuthorizationTrackingStatus.AUTHORIZED)
        {
            Debug.Log("SetAdvertiserTrackingEnabled = true");
            AudienceNetwork.AdSettings.SetAdvertiserTrackingEnabled(true);
        }
#endif


    }

    public void RequestAuthorizationTrackingCompleteHandler(int status)
    {
        Debug.Log($"授权状态:{status}");
        //TODO:根据授权状态设置广告追踪
#if !UNITY_EDITOR && UNITY_IOS
        switch (ATTrackingStatusBinding.GetAuthorizationTrackingStatus())
        {
            case ATTrackingStatusBinding.AuthorizationTrackingStatus.AUTHORIZED:
                Debug.Log("已经授权");
                // AudienceNetwork.AdSettings.SetAdvertiserTrackingEnabled(true);
                break;
            case ATTrackingStatusBinding.AuthorizationTrackingStatus.DENIED:
                Debug.Log("拒绝授权");
                // AudienceNetwork.AdSettings.SetAdvertiserTrackingEnabled(false);
                break;
            default:
                Debug.Log("无状态");
                // AudienceNetwork.AdSettings.SetAdvertiserTrackingEnabled(false);
                break;
        }
#endif
    }

    //af 卸载测量，暂时没有实现
    public void unInstallMeasure(MonoBehaviour monoBehaviour)
    {
        // #if !UNITY_EDITOR && UNITY_ANDROID
        //         //android 暂时没有实现
        // #elif !UNITY_EDITOR && UNITY_IOS
        //         monoBehaviour.StartCoroutine(RequestAuthorization());
        //         Screen.orientation = ScreenOrientation.Portrait;
        // #endif
    }



    public void logIapRevenue()
    {

    }

    public void logAdRevenue()
    {

    }

    IEnumerator RequestAuthorization()
    {
#if !UNITY_ANDROID && !UNITY_EDITOR
        using (var req = new AuthorizationRequest(AuthorizationOption.Alert | AuthorizationOption.Badge, true))
        {

            while (!req.IsFinished)
            {
                yield return null;
            }
            if (req.Granted && req.DeviceToken != "")
            {
                AppsFlyer.registerUninstall(Encoding.UTF8.GetBytes(req.DeviceToken));

            }
        }
#else
        yield return null;
#endif
    }

    public void onConversionDataSuccess(string conversionData)
    {
        Debug.Log("appsflyer onConversionDataSuccess=" + conversionData);
    }

    public void onConversionDataFail(string error)
    {
        Debug.Log("appsflyer onConversionDataFail=" + error);
    }

    public void onAppOpenAttribution(string attributionData)
    {
        Debug.Log("appsflyer onAppOpenAttribution=" + attributionData);
    }

    public void onAppOpenAttributionFailure(string error)
    {
        Debug.Log("appsflyer onAppOpenAttributionFailure=" + error);
    }
}

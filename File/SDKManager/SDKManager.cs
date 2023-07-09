using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SDKManager : Singleton<SDKManager>
{


    /*
        以下是sdk管理，如果有些插件没有用到注释即可
    
    */

    public string productsInfo = "";
    public bool isConnectGoolge = false;

    public void initSDK()  //TODO 初始化sdk，希望尽早调用
    {
        ApplovinImpl.Instance.initMax();//广告初始化
        // IAPManager.Instance.initIAP(); //内购初始化
    }

    public void logEvent(string key, object value)
    {
        if (value == null)
        {
            value = "";
        }
        // FirebaseAnalytics.Instance.logEvent(key, value.ToString()); //firebase打点
        // AppsflyerLog.Instance.logEvent(key, value.ToString());//appsflyer打点
        AliyunManager.Instance.logEvnet(key, value.ToString(), false); //阿里云打点
    }

    public void PaymentById(string productId, Action<bool> action)
    {
        // IAPManager.Instance.pay(productId, action); //内购支付方法
    }

    public void showReward(Action<bool> action, string ActionName)
    {
        ApplovinImpl.Instance.showReward(action, ActionName); //展示激励视频广告
    }

    public void showInterstitial(Action action)
    {
        ApplovinImpl.Instance.showInterstitial(action); //展示插屏广告
    }

    public void setProductInfo(string infos)
    {
        productsInfo = infos;
    }

    public string getProductsInfo()
    {
        return productsInfo;
    }

    //记录广告收入，不需要则注释
    public void logAdRevenue(string ad_platform, string ad_source, string ad_unit_name, string ad_format, double revenue)
    {
        // var impressionParameters = new[] {
        //     new Firebase.Analytics.Parameter("ad_platform", ad_platform),
        //     new Firebase.Analytics.Parameter("ad_source", ad_source),
        //     new Firebase.Analytics.Parameter("ad_unit_name", ad_unit_name),
        //     new Firebase.Analytics.Parameter("ad_format", ad_format),
        //     new Firebase.Analytics.Parameter("value", revenue),
        //     new Firebase.Analytics.Parameter("currency", "USD"), // All AppLovin revenue is sent in USD
        // };
        // Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression", impressionParameters);
        AliyunManager.Instance.logEvnet("ad_impression", "ad_source=" + ad_source + "ad_unit_name=" + ad_unit_name + "revenue=" + revenue, false);
        // double ecpm = revenue * 1000;
        // SDKManager.Instance.logEcpm(ecpm); //记录ecpm

    }

    //firebase 记录内购收入
    public void firebaseLogIapRevenue(string itemId, string itemName, string itemCategory, double value, string currency)
    {
        // var purchaseParameters = new[] {
        //     new Firebase.Analytics.Parameter("item_id", itemId),
        //     new Firebase.Analytics.Parameter("item_name", itemName),
        //     new Firebase.Analytics.Parameter("item_category", itemCategory),
        //     new Firebase.Analytics.Parameter("value", value),
        //     new Firebase.Analytics.Parameter("currency", currency),
        // };
        // Firebase.Analytics.FirebaseAnalytics.LogEvent("purchase", purchaseParameters);
    }

    public void logIapRevenue(string itemId, string itemName, string itemCategory, double price, string currencyCode)
    { //记录内购收入
#if !UNITY_EDITOR
        Debug.Log($"记录收入 itmeId = {itemId}, price={price} , currencyCode={currencyCode}");
        firebaseLogIapRevenue(itemId, itemName, itemCategory, price, currencyCode);
        AppsflyerLog.Instance.logIapRevenue(itemId, itemName, itemCategory, price, currencyCode);
#endif
    }

    //是否登陆方法，用来做验证，firebase插件，不需要则注释
    public bool isShowLogin()
    {
        // if (FirebaseAuthImpl.Instance.currentUser == null)
        // {
        //     return true;
        // }
        // else
        // {
        //     return false;
        // }
        return true;
    }
    //登录插件，不需要则注释
    public void login(Action<bool, string> loginState)
    {
        // GoogleLoginImpl.Instance.LoginWithGoogle(loginState);
    }
    //firebase登录授权，不需要则注释
    public void authLogin(Action<bool> authState, string idToken)
    {
        // FirebaseAuthImpl.Instance.googleAuthWithIdToken(authState, idToken); //使用idToken登录firebase
    }
    //设置用户数据，firebase
    public void setUserData(string data, Action<bool> saveDataState)
    {
        // isLogin((bool isLogin) =>
        // {
        //     if (isLogin)
        //     {
        //         FireStoreImpl.Instance.setUserData(data, saveDataState);
        //     }
        //     else
        //     {
        //         saveDataState.Invoke(false);
        //     }
        // });
    }



    //获取用户数据，firebase，不需要则注释
    public void getUserData(Action<string> getUserDataState)
    {
        // isLogin((bool isLogin) =>
        // {
        //     if (isLogin)
        //     {
        //         FireStoreImpl.Instance.getUserData(getUserDataState);
        //     }
        //     else
        //     {
        //         getUserDataState.Invoke("");
        //     }

        // });

    }
    //firebase 登出
    public void logout()
    {
        // FirebaseAuthImpl.Instance.signOut();
    }

    //记录观看了几次激励视频打点 1，2，3，5，10打点 其他不打点
    

    public void isLogin(Action<bool> loginState)
    {
        // if (FirebaseAuthImpl.Instance.currentUser == null)
        // {
        //     login((bool isOver, string idToken) =>
        //     {
        //         if (isOver)
        //         {
        //             authLogin((bool isSuccess) =>
        //             {
        //                 if (isSuccess)
        //                 {
        //                     loginState.Invoke(true);
        //                 }
        //                 else
        //                 {
        //                     loginState.Invoke(false);
        //                 }
        //             }, idToken);
        //         }
        //         else
        //         {
        //             loginState.Invoke(false);
        //         }
        //     });
        // }
        // else
        // {
        //     loginState.Invoke(true);
        // }
    }

    public bool getGoogleState()
    {
        return isConnectGoolge;
    }

}

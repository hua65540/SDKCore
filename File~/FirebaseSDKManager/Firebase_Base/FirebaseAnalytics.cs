using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseAnalytics : Singleton<FirebaseAnalytics>
{

    public void setUserId(string uid)
    {
        Firebase.Analytics.FirebaseAnalytics.SetUserId(uid);
    }

    public void SetUserProperty(string key, string value)
    { //设置用户属性
        Firebase.Analytics.FirebaseAnalytics.SetUserProperty(key, value);
    }

    public void logEvent(string key, string value)
    { //记录自定义事件
        Firebase.Analytics.FirebaseAnalytics.LogEvent(key, key, value);
    }

    //记录广告收入
    public void logAdRevenue(string ad_platform, string ad_source, string ad_unit_name, string ad_format, double value, string currency)
    {
        var adRevenueParameters = new[] {
            new Firebase.Analytics.Parameter("ad_platform", ad_platform),
            new Firebase.Analytics.Parameter("ad_source", ad_source),
            new Firebase.Analytics.Parameter("ad_unit_name", ad_unit_name),
            new Firebase.Analytics.Parameter("ad_format", ad_format),
            new Firebase.Analytics.Parameter("value", value),
            new Firebase.Analytics.Parameter("currency", currency),
        };
        Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_revenue", adRevenueParameters);
    }
    //记录内购收入
    public void logIapRevenue(string itemId, string itemName, string itemCategory, double value, string currency)
    {
        var purchaseParameters = new[] {
            new Firebase.Analytics.Parameter("item_id", itemId),
            new Firebase.Analytics.Parameter("item_name", itemName),
            new Firebase.Analytics.Parameter("item_category", itemCategory),
            new Firebase.Analytics.Parameter("value", value),
            new Firebase.Analytics.Parameter("currency", currency),
        };
        Firebase.Analytics.FirebaseAnalytics.LogEvent("purchase", purchaseParameters);
    }

}

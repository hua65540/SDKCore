using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppsFlyerSDK;

public class AppsflyerLog : Singleton<AppsflyerLog>
{
    public void logEvent(string key, string value)
    {
        Dictionary<string, string> eventValues = new Dictionary<string, string>();
        eventValues.Add(key, value);
        AppsFlyer.sendEvent(key, eventValues);
    }

    public void logIapRevenue(string itemId, string itemName, string itemCategory, double price, string currencyCode)
    {
        Dictionary<string, string> eventValues = new Dictionary<string, string>();
        eventValues.Add(AFInAppEvents.CURRENCY, currencyCode);
        eventValues.Add(AFInAppEvents.REVENUE, price.ToString());
        eventValues.Add("af_quantity", "1");
        AppsFlyer.sendEvent(AFInAppEvents.PURCHASE, eventValues);
    }
}

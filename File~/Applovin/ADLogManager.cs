using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADLogManager : Singleton<ADLogManager>
{
    
    public void clickEvent(string key,string value){
        
    }

    //这是记录广告收入的方法，需要引入firebase分析sdk，引入之后即可正常使用，不需要则注释掉firebase部分即可
    public void revenueEvent(string ad_platform,string ad_source,string ad_unit_name,string ad_format,double revenue){
        // var impressionParameters = new[] {
        //             new Firebase.Analytics.Parameter("ad_platform", ad_platform),
        //             new Firebase.Analytics.Parameter("ad_source", ad_source),
        //             new Firebase.Analytics.Parameter("ad_unit_name", ad_unit_name),
        //             new Firebase.Analytics.Parameter("ad_format", ad_format),
        //             new Firebase.Analytics.Parameter("value", revenue),
        //             new Firebase.Analytics.Parameter("currency", "USD"), // All AppLovin revenue is sent in USD
        //         };
        // Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression", impressionParameters);
        // //下面是aliyun记录，如果没有引入aliyun则一起注释掉
        // AliyunManager.Instance.logEvnet("ad_impression", "ad_source=" + ad_source + "ad_unit_name=" + ad_unit_name + "revenue=" + revenue, false);
    }

    public void rewardShowEvent(string key,string value){
        
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Params
{

    public static bool isSetFirebaseCrashlytics = true; //开始firebase 崩溃日志收集
    
    
    //appsflyer sdk start
    public static string af_sdk_key_android = "vSxVtakprXVJjy9hBFTvQB";
    public static string af_sdk_key_ios = "";
    public static string af_app_id_ios = "";
    public static bool af_isDebug = false; //是否开启appsflyer debug模式
    //appsflyer sdk end
    
    //applovin sdk start
    public static string applovin_sdk_key_ios = "Swc-59Bra-vHHOI73JUTVByNcyHs0PFVh7GpWpm6R0DWMweTgPpiiK6_ZM8KTPNqT9TMeupc57TLuXOwWtCItl";
    public static string applovin_sdk_key_android = "Swc-59Bra-vHHOI73JUTVByNcyHs0PFVh7GpWpm6R0DWMweTgPpiiK6_ZM8KTPNqT9TMeupc57TLuXOwWtCItl";
    //applovin sdk end
    
    //aliyun start
    public static string aliyun_appId = "Survivor";
    public static string aliyun_base_url = "https://mountain-abroad.us-west-1.log.aliyuncs.com";
    //aliyun end
    
    //firebase 存储结构
    public static string store_collection_userInfo = "UserInfo";
    public static string store_doc_userInfo = "UserInfo";
    public static string store_doc_userData = "UserData";
    public static string key_user_id = "user_id";
    public static string server_client_id = "736893665228-3fvlvd00e8vm255frmo4870l8po73nj3.apps.googleusercontent.com";
    public static string reward_action_name = "";
    public static string interstitial_action_name = "";
    public static string playerPrefs_adEcpm = "adEcpm";
    public static string reward_id_android = "bb989c1db1c7f754";
    public static string reward_id_ios = "";
    public static string interstitial_id_android = "";
    public static string interstitial_id_ios = "";



}

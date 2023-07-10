using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Field
{
   
    public static string backupDir = Path.GetFullPath("Packages/com.skyisland.sdk_core/File");
    public static string baseDir = "./Assets/SDKCore";


    //------------目标文件
    public static string backupDirAliyun = backupDir+"~/Aliyun";
    public static string baseDirAliyun = baseDir+"/Aliyun";
    public static string backupDirSDKManager = backupDir+"~/SDKManager";
    public static string baseDirSDKManager = baseDir+"/SDKManager";
    public static string backupDirApplovin = backupDir+"~/Applovin";
    public static string baseDirApplovin = baseDir+"/Applovin";
    public static string backupDirAppsflyerManager = backupDir+"~/AppsflyerManager";
    public static string baseDirDirAppsflyerManager = baseDir+"/AppsflyerManager";
    public static string backupDirFirebaseBase = backupDir+"~/FirebaseBase";
    public static string baseDirFirebaseBase = baseDir+"/FirebaseBase";
    public static string backupDirIAP = backupDir+"~/IAP";
    public static string baseDirIAP = "/IAP";

    //----------------------插件
    public static string backupDirAppsflyer = backupDir+"~/AppsFlyer"; //appsflyer 插件
    public static string baseDirAppsflyer = baseDir+"/AppsFlyer"; //appsflyer 插件
    public static string backupDirmaxSDK = backupDir+"~/MaxSdk"; //Applovin 插件
    public static string baseDirmaxSDK = baseDir+"/MaxSdk"; //Applovin 插件
    public static string backupDirPlayServicesResolver = backupDir+"~/PlayServicesResolver"; //前置文件
    public static string baseDirPlayServicesResolver = baseDir+"/PlayServicesResolver"; //前置文件
    public static string backupDirFirebase = backupDir+"~/FirebasePlugin";
    public static string baseDirFirebase = baseDir+"/FirebasePlugin";





}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Menu : EditorWindow
{
    
    
    

    public const string menu = "SDKCore/";
    public const string load = "load";
    public const string reload = "reload";

    public const string aliyun = "Aliyun/";
    public const string applovin = "Applovin/";
    public const string appsflyer = "Appsflyer/";
    public const string firebaseBase = "Firebase/Analytics/";
    public const string iap = "IAP/";
    
    [MenuItem(menu+aliyun+load)]
    private static void AliyunLoad()
    {
        Debug.Log("导入aliyun");
        importFront();
        CopyFolder(Field.backupDirAliyun,Field.baseDirAliyun);
    }

    [MenuItem(menu+aliyun+reload)]
    private static void AliyunReload()
    {
        Debug.Log("重新导入aliyun");
        reloadFile(Field.backupDirAliyun, Field.baseDirAliyun);
    }

    [MenuItem(menu+applovin+load)]
    private static void ApplovinLoad(){
        Debug.Log("导入applovin");
        importFront();
        CopyFolder(Field.backupDirApplovin,Field.baseDirApplovin);
        CopyFolder(Field.backupDirmaxSDK,Field.baseDirmaxSDK);
        
    }


    [MenuItem(menu+applovin+reload)]
    private static void ApplovinReLoad(){
        Debug.Log("重新导入applovin,重新导入applovin不会更新插件,只会更改代码");
        reloadFile(Field.backupDirApplovin,Field.baseDirApplovin);
    }

    [MenuItem(menu+appsflyer+load)]
    private static void AppsflyerLoad(){
        Debug.Log("导入appsflyer");
        importFront();
        CopyFolder(Field.backupDirAppsflyerManager,Field.baseDirDirAppsflyerManager);
        CopyFolder(Field.backupDirAppsflyer,Field.baseDirAppsflyer);
        
    }


    [MenuItem(menu+appsflyer+reload)]
    private static void AppsflyerReload(){
        Debug.Log("重新导入appsflyer");
        reloadFile(Field.backupDirAppsflyerManager,Field.baseDirDirAppsflyerManager);
    }

    [MenuItem(menu+firebaseBase+load)]
    private static void FirebaseBaseLoad(){
        Debug.Log("导入firebase");
        importFront();
        CopyFolder(Field.backupDirFirebaseBase,Field.baseDirFirebaseBase);
        CopyFolder(Field.backupDirFirebase,Field.baseDirFirebase);
        
    }


    [MenuItem(menu+firebaseBase+reload)]
    private static void FirebaseBaseReload(){
        Debug.Log("重新导入firebase,firebase重新导入会把插件一起重新导入");
        reloadFile(Field.backupDirFirebaseBase,Field.baseDirFirebaseBase);
        reloadFile(Field.backupDirFirebase,Field.backupDirFirebase);
    }
    
    [MenuItem(menu+iap+load)]
    private static void IAPLoad(){
        Debug.Log("导入内购,只会导入逻辑部分,需要手动导入依赖包,请看说明文档IAP部分");
        importFront();
        CopyFolder(Field.backupDirIAP,Field.baseDirIAP);
    }


    [MenuItem(menu+iap+reload)]
    private static void IAPReload(){
        Debug.Log("重新导入firebase,firebase重新导入会把插件一起重新导入");
        reloadFile(Field.backupDirIAP,Field.baseDirIAP);
    }
    
    public static void reloadFile(string sourceFolder, string destFolder)
    {
        if (System.IO.Directory.Exists(destFolder))
        {
            System.IO.Directory.Delete(destFolder);
        }
        CopyFolder(sourceFolder,destFolder);
    }



    
    

    // 复制文件夹
    public static void CopyFolder(string sourceFolder, string destFolder)
    {
        try
        {
            
            //如果目标路径不存在,则创建目标路径
            if (!System.IO.Directory.Exists(destFolder))
            {
                System.IO.Directory.CreateDirectory(destFolder);
            }
            //得到原文件根目录下的所有文件
            string[] files = System.IO.Directory.GetFiles(sourceFolder);
            foreach (string file in files)
            {
                string name = System.IO.Path.GetFileName(file);
                string dest = System.IO.Path.Combine(destFolder, name);
                System.IO.File.Copy(file, dest);//复制文件
            }
            //得到原文件根目录下的所有文件夹
            string[] folders = System.IO.Directory.GetDirectories(sourceFolder);
            foreach (string folder in folders)
            {
                string name = System.IO.Path.GetFileName(folder);
                string dest = System.IO.Path.Combine(destFolder, name);
                CopyFolder(folder, dest);//构建目标路径,递归复制文件
            }
        }
        catch (System.Exception e)
        {
            Debug.Log("复制文件出现异常,msg="+e.Message);
        }
    }


    //导入前置文件
    public static void importFront(){
        if(System.IO.Directory.Exists(Field.baseDirSDKManager)){
            Debug.Log("已有前置文件,如果需要重新导入,请手动删除SDKManager文件");
            return;
        }
        CopyFolder(Field.backupDirPlayServicesResolver,Field.baseDirPlayServicesResolver);
        CopyFolder(Field.backupDirSDKManager,Field.baseDirSDKManager);
    }
    
}

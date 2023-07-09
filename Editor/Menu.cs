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
        CopyFolder(Field.backupDirPlayServicesResolver,Field.baseDirPlayServicesResolver);
    }


    [MenuItem(menu+applovin+reload)]
    private static void ApplovinReLoad(){
        Debug.Log("重新导入applovin");
        reloadFile(Field.backupDirApplovin,Field.baseDirApplovin);
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
        CopyFolder(Field.backupDirSDKManager,Field.baseDirSDKManager);
    }
    
}

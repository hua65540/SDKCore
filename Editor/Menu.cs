using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Menu : EditorWindow
{
    
    public static string backupDir = Path.GetFullPath("Packages/com.skyisland.sdk_core/File");
    public static string baseDir = "./Assets/SDKCore";
    

    public const string menu = "SDKCore/";
    public const string load = "load";
    public const string reload = "reload";

    public const string aliyun = "Aliyun/";
    
    [MenuItem(menu+aliyun+load)]
    private static void ShowWindow()
    {
        Debug.Log("导入文件");
        importFront();
        CopyFolder(backupDir+Field.aliyun, baseDir+Field.aliyun);
    }

    [MenuItem(menu+aliyun+reload)]
    private static void ShowWindow1()
    {
        Debug.Log("重新导入文件");
        reloadFile(backupDir, baseDir);
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
        if(System.IO.Directory.Exists(baseDir+Field.sdkmanager)){
            Debug.Log("已有前置文件,如果需要重新导入,请手动删除SDKManager文件");
            return;
        }
        CopyFolder(backupDir+Field.sdkmanager,baseDir+Field.sdkmanager);
    }
    
}

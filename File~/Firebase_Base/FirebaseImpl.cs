using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;


public class FirebaseImpl : MonoBehaviour
{

    public FirebaseApp app;

    void Awake()
    {
        Debug.Log("firebase init");
        DontDestroyOnLoad(this);
        GoogleLoginImpl.Instance.setGoogleLoginWithSCId(); //设置google登录的server_client_id
#if !UNITY_EDITOR && UNITY_ANDROID
            checkGooglePlayVerson(); //初始化firebase
#elif !UNITY_EDITOR && UNITY_IOS
            init();
#endif
    }

    private void Start()
    {

    }

    public void init()
    {
        FirebaseCrashlyticsImpl.Instance.init();//开始收集崩溃报告
        FirebaseAuthImpl.Instance.init(); //初始化firebase auth
        LogEventManager.Instance.GameSDKInit(); //初始化游戏sdk打点

    }

    public void checkGooglePlayVerson()
    {
        // Initialize Firebase
        Debug.Log("检查google play更新...");
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                Debug.Log("google play更新检查完成...");
                app = FirebaseApp.DefaultInstance;
                init();
            }
            else
            {
                // Firebase Unity SDK is not safe to use here.
                Debug.Log($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
    }




}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Crashlytics;

public class FirebaseCrashlyticsImpl : Singleton<FirebaseCrashlyticsImpl>
{
    public void init()
    {
        Crashlytics.ReportUncaughtExceptionsAsFatal = Params.isSetFirebaseCrashlytics; //开始收集崩溃报告
    }

    public void testCrash()
    {
        Crashlytics.Log("testCrash");
        Crashlytics.LogException(new System.Exception("testCrash"));
    }

}

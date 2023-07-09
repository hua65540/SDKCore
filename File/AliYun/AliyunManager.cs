using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.Concurrent;
using System.Globalization;

public class AliyunManager : Singleton<AliyunManager>
{

    private static Queue<AliyunEntity> aLiYunQueue;
    private static bool queueStatus = false;
    private static ConcurrentQueue<AliyunEntity> aLiYunConcurrentQueue;
    private static AliyunEntity tempALiYunEntity;
    public Action<int> aLiYunStatus;
    public bool isInit = false;


    public void init()
    {
        AliYunAnalytics.Instance.Init(appId: Params.aliyun_appId, engine: "u_3d", platform: "android", cversion: "1.0.0", channel: "google", loginDays: Utils.Instance.GetLoginDay().ToString(), app_version: Application.version, country: RegionInfo.CurrentRegion.Name);
        isInit = true;
    }

    public void logEvnet(string key, string value, bool isDebug)
    {
        if (isDebug)
        {
            EnQueue(key, value);
        }
        else
        {
            EnQueue(key, value);
        }
    }



    public void EnQueue(string key, string value)
    {

        if (aLiYunConcurrentQueue == null)
        {
            aLiYunConcurrentQueue = new ConcurrentQueue<AliyunEntity>();
        }
        AliyunEntity aliyunEntity = new AliyunEntity();
        aliyunEntity.SetKey(key);
        aliyunEntity.SetValue(value);
        aLiYunConcurrentQueue.Enqueue(aliyunEntity);
        Debug.Log($"安全队列_入队:{queueStatus},event:{aliyunEntity.GetKey()},data:{aliyunEntity.GetValue()}");

        if (isInit) //只有初始化完成才会走队列
        {
            if (!queueStatus)
            {
                queueStatus = true;
                PushALiYunLogEvent();
            }
        }
    }

    //发送打点，进行出队操作，如果队列中没有打点任务，则把队列状态改为false，表示此时没有打点任务
    public void PushALiYunLogEvent()
    {

        if (aLiYunConcurrentQueue != null)
        {
            if (aLiYunConcurrentQueue.Count > 0)
            {
                //安全队列 出队
                aLiYunConcurrentQueue.TryDequeue(out tempALiYunEntity);
                Debug.Log($"安全队列_出队:event:{tempALiYunEntity.GetKey()},data:{tempALiYunEntity.GetValue()}");
                SendLogEventListener();
            }
            else
            {
                queueStatus = false;
            }
        }

    }

    //递归操作，根据打点成功或失败来进行下一个或重试
    public void SendLogEventListener()
    {
        SendLogEvent(delegate (int code)
        {
            switch (code)
            {
                case 001:
                    PushALiYunLogEvent();
                    break;
                case 002:
                    SendLogEventListener();
                    break;
            }
        });
    }

    public void SendLogEvent(Action<int> aLiYunStatus)
    {
        if (tempALiYunEntity != null)
        {
            this.aLiYunStatus = aLiYunStatus;
            AliYunAnalytics.Instance.SendEvent(tempALiYunEntity.GetKey(), tempALiYunEntity.GetValue());
        }

    }

    class AliyunEntity
    {
        private string key;
        private string value;

        public void SetKey(string key)
        {
            this.key = key;
        }

        public string GetKey()
        {
            return key;
        }

        public void SetValue(string value)
        {
            this.value = value;
        }

        public string GetValue()
        {
            return value;
        }
    }

}

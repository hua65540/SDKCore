using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System.Net;

public class AliYunAnalytics
{

    static readonly string VERSION = "1.4.0";

    static readonly string BASE_URL = Params.aliyun_base_url;
    static readonly string LOG_REPORT_PATH = "/logstores/log/track";
    static readonly string FIRST_LOGIN_DATE_KEY = "FIRST_LOGIN_DATE";
    static readonly string PID_KEY = "PID_KEY";
    static readonly string DAYS_FROM_FIRST_LOGIN_KEY = "DAYS_FROM_FIRST_LOGIN";
    private bool isInit = false;
    private string appId = "appId";
    private string engine = "unity";
    private string platform = "android";
    private string cversion = "1";
    private string loginDays = "1";
    private string app_version = "0.1";
    private string country = "cn";



    private string channel = "channel";
    private string sid = null;     // sessionId
    private string pid = null;     // playerId
    private string firstLoginDate = null;
    public int daysFromFirstLogin = 0;
    private List<LogEvent> eventsQueue;
    public URL logReportUrl = null;

    public URL url = null;

    public bool isFirst = true;

    private bool isReporingStarted = false;

    private static AliYunAnalytics sharedInstance = null;
    public static AliYunAnalytics Instance
    {
        get
        {
            if (sharedInstance == null)
            {
                sharedInstance = new AliYunAnalytics();
            }
            return sharedInstance;
        }


    }


    public void initUrl()
    {
        //reportEvent();
        // #region Generate session id
        long now1 = DateTime.Now.ToFileTimeUtc();
        long rand = (long)Math.Floor(1e6 * (new System.Random().Next()));
        sid = (now1 + rand).ToString();

    }


    //FIRST_LOGIN_DATE_KEY 初始时间
    //daysFromFirstLogin 登陆第几天
    public void SendEvent(string evname = null, string ev = null)
    {
        try
        {
            if (appId != "")
            {
                pid = PlayerPrefs.GetString(PID_KEY);
                if (pid == "")
                {
                    pid = appId + sid;
                    PlayerPrefs.SetString(PID_KEY, pid);
                }
            }
            //reportEvent();
            // #region Generate session id
            // long now1 = DateTime.Now.ToFileTimeUtc();
            // long rand = (long)Math.Floor(1e6 * (new System.Random().Next()));
            // sid = (now1 + rand).ToString();
            // #endregion
            // #region Setup user first login date
            firstLoginDate = PlayerPrefs.GetString(AliYunAnalytics.FIRST_LOGIN_DATE_KEY);
            if (firstLoginDate == "")
            {
                //firstLoginDate = formatDate(new DateTime());
                firstLoginDate = DateTime.Now.DayOfYear.ToString();

                PlayerPrefs.SetString(AliYunAnalytics.FIRST_LOGIN_DATE_KEY, firstLoginDate);
            }
            // #endregion
            // #region Setup days from first login
            daysFromFirstLogin = PlayerPrefs.GetInt(
                AliYunAnalytics.DAYS_FROM_FIRST_LOGIN_KEY
        );
            if (daysFromFirstLogin == 0)
            {
                int now = DateTime.Now.DayOfYear;
                int f = int.Parse(firstLoginDate);
                daysFromFirstLogin = now - f + 1;
                PlayerPrefs.SetInt(AliYunAnalytics.DAYS_FROM_FIRST_LOGIN_KEY, daysFromFirstLogin);
            }
            // #endregion

            // #region Setup channel
            //string launchOptions = getLaunchOptionsSync();


            //reportLaunchScene(int.Parse(launchOptions));//loginScene
            //string wxgamecid = launchOptions ?? "natural";

            //channel = JsonUtility.ToJson(new string[] { wxgamecid });
            // #endregion

            // #region Setup player id
            //string accInfo = getAccountInfoSync();

            //appId = accInfo;

            // #endregion
            // #region Create log report url
            url = new URL(BASE_URL, LOG_REPORT_PATH);
            url.setQuery("version", AliYunAnalytics.VERSION);
            url.setQuery("APIVersion", "0.6.0");
            url.setQuery("fingerprintId", this.sid);
            url.setQuery("createdAt", this.firstLoginDate);
            url.setQuery("loginDays", this.loginDays);
            url.setQuery("appVersion", this.app_version);
            url.setQuery("country", this.country);
            url.setQuery("channel", this.channel);
            url.setQuery("appid", this.appId);
            url.setQuery("engine", this.engine);
            url.setQuery("platform", this.platform);
            url.setQuery("cversion", this.cversion);
            url.setQuery("playerId", this.pid);

            logReportUrl = url;
            reportInit(evname, ev);
        }
        catch (System.Exception e)
        {
            Debug.Log("设置query出错" + e.Message);
            AliyunManager.Instance.aLiYunStatus.Invoke(002);
        }
    }

    //启动参数，自定义
    string getLaunchOptionsSync()
    {
        string launchOption = "";
        return launchOption;
    }
    //玩家id，自定义
    string getAccountInfoSync()
    {
        long now1 = DateTime.Now.ToFileTimeUtc();
        long rand = (long)Math.Floor(1e6 * (new System.Random().Next()));
        string info = "";
        return info;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="appId"></param>
    /// <param name="engine"></param>
    /// <param name="platform"></param>
    /// <param name="cversion"></param>
    /// <param name="channel"></param>
    public void Init(string appId = "appId", string engine = "engine", string platform = "platform", string cversion = "cversion", string channel = "channel", string loginDays = "1", string app_version = "0.1", string country = "cn")
    {
        this.appId = appId;
        this.engine = engine;
        this.platform = platform;
        this.cversion = cversion;
        this.channel = channel;
        this.loginDays = loginDays;
        this.app_version = app_version;
        this.country = country;
        this.isInit = true;
        initUrl();
    }

    private void reportLaunchScene(int scene)
    {
        this.logEvent("loginScene", scene.ToString());
    }

    private void reportInit(string event1, string data1)
    {
        _reportEvent(new LogEvent(event1, data1));

    }
    void logEvent(string eventName, string params11 = null)
    {
        eventsQueue.Add(new LogEvent(eventName, params11));
        if (eventsQueue.Count > 200)
        {
            Debug.LogWarning("Log events buffered over 200!");
            eventsQueue.RemoveAt(0);
        }

        if (!isReporingStarted)
        {
            isReporingStarted = true;
            reportEvent();
        }
    }
    async Task reportEvent()
    {
        if (pid == null || channel == null)
        {
            Thread.Sleep(1000);
            await reportEvent();

        }
        Thread.Sleep(1000);
        await _reportEvent(eventsQueue[0]);//等待


        eventsQueue[0] = null;
        eventsQueue.RemoveAt(0);

        if (eventsQueue.Count > 0)
        {
            Thread.Sleep(100);
            await reportEvent();
        }
        else
        {
            isReporingStarted = false;
        }
    }

    #region HttpWebRequest异步GET
    public static void AsyncGetWithWebRequest(string url)
    {
        var request = (HttpWebRequest)WebRequest.Create(new Uri(url));
        request.BeginGetResponse(new AsyncCallback(ReadCallback), request);
    }
    private static void ReadCallback(IAsyncResult asynchronousResult)
    {
        var request = (HttpWebRequest)asynchronousResult.AsyncState;
        var response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
        using (var streamReader = new StreamReader(response.GetResponseStream()))
        {
            var resultString = streamReader.ReadToEnd();
            Debug.Log("GET" + resultString);
        }
    }
    #endregion

    #region WebClient异步GET

    public static void AsyncGetWithWebClient(string url)
    {
        var webClient = new WebClient();
        // HttpRestful.Instance.Get(url, delegate (bool flag,string msg)
        // {
        //     Debug.Log("--------------失败 falg="+flag+":错误信息="+msg);
        // }, delegate
        // {
        //     Debug.Log("-------------成功发送打点");
        // });
        // UnityWebRequest.Post();
        // webClient
        // webClient.url = url;
        // webClient.method = "POST";
        // webClient.SendWebRequest();
        // Debug.Log("---------------结果="+webClient.result);
        //
        // Http.Get(url);
        // var webClient = new WebClient();

        webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
        webClient.DownloadStringAsync(new Uri(url));
    }

    private static void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
    {
        Debug.Log(e.Error != null ? "WebClient异步GET发生错误！error=" + e.Error : "GET:" + e.Result);
        bool logEventFailed = e.Error != null ? true : false;
        if (logEventFailed)
        {
            //打点发送失败，重试
            AliyunManager.Instance.aLiYunStatus.Invoke(002);
        }
        else
        {
            //成功发送打点
            AliyunManager.Instance.aLiYunStatus.Invoke(001);
        }

    }

    #endregion
    async Task _reportEvent(LogEvent ev)
    {
        URL url = logReportUrl;
        url.setQuery("event", ev.eventName);
        url.setQuery("data", ev.params1);
        List<string> a = new List<string>();
        foreach (string item in url.query.Keys)
        {
            //Debug.Log("Report Event: " + item+"   "+url.query[item]);
            a.Add(item + "=" + url.query[item]);
        }
        //Debug.Log(url.path + url.base1 + "?" + string.Join("&", a));
        //SK.AsyncGetWithWebRequest(url.path + url.base1 + "?" + string.Join("&", a));
        AliYunAnalytics.AsyncGetWithWebClient(url.path + url.base1 + "?" + string.Join("&", a));
    }

    private string formatDate(DateTime date, bool long1 = false)
    {
        int y = date.Year;
        int M = date.Month + 1;
        int d = date.Day;

        if (long1)
        {
            int h = date.Hour;
            int m = date.Minute;
            int s = date.Second;
            int ms = date.Millisecond;

            return y.ToString() + "-" + (M >= 10 ? M.ToString() : "0" + M.ToString()) + "-" + d.ToString() + ":" + h.ToString() + ":" +
            m.ToString() + ":" + s.ToString() + ":" + ms.ToString();
        }
        else
        {
            return y.ToString() + "-" + (M >= 10 ? M.ToString() : "0" + M.ToString()) + "-" + d.ToString();
        }
    }
}
class LogEvent
{
    public string eventName;
    public string params1;
    public LogEvent(string eventName, string params1)
    {
        this.eventName = eventName;
        this.params1 = params1;
    }

}
public class URL
{
    public string base1;
    public string path;
    public Dictionary<string, string> query;
    public URL(string path, string base1)
    {
        this.path = path;
        this.base1 = base1;
        query = new Dictionary<string, string>();
    }
    public void setQuery(string key, string val)
    {
        try
        {
            query.Add(encodeURI(key), encodeURI(val));
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            AliyunManager.Instance.aLiYunStatus.Invoke(002);
        }

    }

    public static string encodeURI(string str)
    {

        StringBuilder sb = new StringBuilder();
        byte[] byStr = System.Text.Encoding.UTF8.GetBytes(str); //默认是System.Text.Encoding.Default.GetBytes(str)
        for (int i = 0; i < byStr.Length; i++)
        {
            sb.Append(@"%" + Convert.ToString(byStr[i], 16));
        }
        // Debug.Log("str=" + str + "|十六进制=" + sb.ToString());
        return (Convert.ToString(sb));
    }
}

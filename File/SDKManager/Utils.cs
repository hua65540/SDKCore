using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;


public class Utils : Singleton<Utils>
{



    //获取登录天数
   public int GetLoginDay()
    {
        string firstDayStr = PlayerPrefs.GetString("FirstLoginDay", String.Empty);
        DateTime dt = DateTime.Now;
        if (firstDayStr == String.Empty)
        {
            Debug.Log(dt.ToString());
            PlayerPrefs.SetString("FirstLoginDay", dt.ToString());
            return 1;
        }
        DateTime firstDay = Convert.ToDateTime(firstDayStr);
        int day = Math.Abs(((TimeSpan)(dt - firstDay)).Days);
        return day + 1;
    }
}

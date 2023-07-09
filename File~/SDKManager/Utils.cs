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

    public void recordAdCount(){
        int count = getCount(Params.playerPrefs_adCount);
        for(int i = 0;i < Params.rewardCount.Count;i++){
            if(count == Params.rewardCount[i]){
                
            }
        }
    }

    public void ecpmEvent(int count){
        
    }

    public void recordEcpmEvg(double revenue){
        double ecpm = revenue * 1000;
        double player_ecpm = Convert.ToDouble(PlayerPrefs.GetString(Params.playerPrefs_adEcpm,"0"));
        int count = getCount(Params.playerPrefs_ecpmCount);
        double title_ecpm = ecpm+player_ecpm;
        PlayerPrefs.SetString(Params.playerPrefs_adEcpm,title_ecpm.ToString());
    }

    public int getCount(string key){
        int count = PlayerPrefs.GetInt(key,0);
        count++;
        PlayerPrefs.SetInt(key,count);
        return count;
    }



}

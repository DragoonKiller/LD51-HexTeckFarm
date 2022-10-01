using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prota;
using UnityEngine.UI;

public class WeatherText : MonoBehaviour
{
    Text text => this.GetComponent<Text>();
    
    void Update()
    {
        if(Weather.instance.cur == -1)
        {
            text.text = "---";
            return;
        }
        
        var displayTime = ((9.9999f - (Time.time - Weather.instance.lastChangeTime)) * 10).Floor() / 10;
        text.text = displayTime.Max(0).ToString("0.0");
    }
}
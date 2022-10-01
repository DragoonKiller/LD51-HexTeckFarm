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
        var displayTime = ((9.9999f - (Time.time - Weather.Get().lastChangeTime)) * 10).Floor() / 10;
        text.text = displayTime.Max(0).ToString("0.0");
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prota.Unity;
using Prota.Timer;
using System;
using Prota.Tween;

[Serializable]
public enum WeatherType
{
    None = 0,
    Sunny,
    Rain,
    FertilizerRain,
    ThunderStrom,
    Drought,
    Flood,
    Done,
}


[Serializable]
public class WeatherLightColor
{
    public Color none;
    public Color sunny;
    public Color rain;
    public Color fertilizerRain;
    public Color thunderStrom;
    public Color drought;
    public Color flood;
    public Color done;
}


public class Weather : Singleton<Weather>
{
    public WeatherType currentWeather => seq.Count < 0 || cur >= seq.Count ? WeatherType.None : seq[cur];
    
    public List<WeatherType> seq = new List<WeatherType>();
    
    public WeatherLightColor lightColor;
    
    public new Light light;
    
    public int cur = 0;
    
    public Timer timer = null;
    
    public float lastChangeTime = 0;
    
    public bool started = false;
    
    public void Reset()
    {
        started = false;
        cur = -1;
        seq.Clear();
        GenerateSeq();
        timer?.Remove();
        this.light.color = GetLightColor(WeatherType.Sunny);
    }
    
    void Start() => Reset();

    private void GenerateSeq()
    {
        seq.Add(WeatherType.Sunny);
        seq.Add(WeatherType.Sunny);
        seq.Add(WeatherType.Rain);
        seq.Add(WeatherType.FertilizerRain);
        for(int i = 4; i < 31; i++)  // 5min.
        {
            var n = UnityEngine.Random.Range(0, 60);
            // n.ToString().Log();
            WeatherType w = WeatherType.None;
            if(n < 10) w = WeatherType.Sunny;
            else if(n < 20) w = WeatherType.Rain;
            else if(n < 30) w = WeatherType.FertilizerRain;
            else if(n < 40) w = WeatherType.ThunderStrom;
            else if(n < 50) w = WeatherType.Flood;
            else if(n < 60) w = WeatherType.Drought;
            seq.Add(w);
        }
        seq.Add(WeatherType.Done);
    }

    void Update()
    {
        if(started) return;
        bool hasPlanted = false;
        foreach(var b in Ground.instance.blocks) hasPlanted |= b?.plant != null;
        if(hasPlanted)
        {
            started = true;
            NextWeather();
            timer = Timer.New(10, true, NextWeather);        // the actual start.
        }
    }
    
    void NextWeather()
    {
        cur += 1;
        lastChangeTime = Time.time;
        
        var sourceColor = this.light.color;
        ProtaTweenManager.instance.New(TweenType.Transparency, this.light, (h, t) => {
            var targetColor = GetLightColor(this.currentWeather);
            this.light.color = (sourceColor, targetColor).Lerp(t);
        }).SetEase(TweenEase.quadInOut).SetFrom(0).SetTo(1).Start(1);
    }
    
    public Color GetLightColor(WeatherType type)
    {
        return type switch {
            WeatherType.Done => lightColor.done,
            WeatherType.Rain => lightColor.rain,
            WeatherType.Drought => lightColor.drought,
            WeatherType.Sunny => lightColor.sunny,
            WeatherType.Flood => lightColor.flood,
            WeatherType.FertilizerRain => lightColor.fertilizerRain,
            WeatherType.ThunderStrom => lightColor.thunderStrom,
            _ => lightColor.none,
        };
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prota.Unity;
using Prota.Timer;
using System;
using Prota.Tween;
using System.Linq;
using Prota;

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
        
        var validCollection = new Dictionary<WeatherType, int>();
        validCollection.Add(WeatherType.Sunny, 10);
        validCollection.Add(WeatherType.Rain, 10);
        validCollection.Add(WeatherType.FertilizerRain, 8);
        validCollection.Add(WeatherType.ThunderStrom, 8);
        validCollection.Add(WeatherType.Flood, 6);
        validCollection.Add(WeatherType.Drought, 6);
        
        var last = WeatherType.None;
        
        for(int i = 4; i < 31; i++)  // 5min. 
        {
            var c = validCollection.Clone();
            c.Remove(last);     // no repeated weather.
            // no continuous flood or drought.
            if(last == WeatherType.Flood) c.Remove(WeatherType.Drought);
            if(last == WeatherType.Drought) c.Remove(WeatherType.Flood);
            
            var sum = c.Values.Sum();
            var n = UnityEngine.Random.Range(0, sum);
            WeatherType w = WeatherType.None;
            foreach(var t in c)
            {
                if(n < t.Value)
                {
                    w = t.Key;
                    break;
                }
                n -= t.Value;
            }
            
            (w != WeatherType.None).Assert();
            seq.Add(last = w);
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

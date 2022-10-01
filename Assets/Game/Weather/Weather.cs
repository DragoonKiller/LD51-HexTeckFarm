using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prota.Unity;
using Prota.Timer;
using System;

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

public class Weather : Singleton<Weather>
{
    public WeatherType currentWeather => seq.Count < 0 || cur >= seq.Count ? WeatherType.None : seq[cur];
    
    public List<WeatherType> seq = new List<WeatherType>();
    
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
    }
    
    void Start() => Reset();

    private void GenerateSeq()
    {
        for(int i = 0; i < 30; i++)  // 5min.
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
    }
    
    
}

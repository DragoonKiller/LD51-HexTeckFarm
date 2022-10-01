using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prota;
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
}

public class Weather : Singleton<Weather>
{
    public WeatherType currentWeather => seq.Count == 0 || cur >= seq.Count ? WeatherType.None : seq[cur];
    
    public List<WeatherType> seq = new List<WeatherType>();
    
    public int cur = 0;
    
    public Timer timer = null;
    
    public float lastChangeTime = 0;
    
    void Start()
    {
        cur = 0;
        seq.Clear();
        GenerateSeq();
        timer?.Remove();
        timer = Timer.New(10, true, NextWeather);
    }

    private void GenerateSeq()
    {
        for(int i = 0; i < 12; i++)
        {
            var n = UnityEngine.Random.Range(0, 50);
            // n.ToString().Log();
            WeatherType w = WeatherType.None;
            if(n < 10) w = WeatherType.Sunny;
            else if(n < 20) w = WeatherType.Rain;
            else if(n < 30) w = WeatherType.FertilizerRain;
            else if(n < 40) w = WeatherType.ThunderStrom;
            else if(n < 50) w = WeatherType.Flood;
            seq.Add(w);
        }
    }

    public void Restart() => Start();
    
    void Update()
    {
        
    }
    
    void NextWeather()
    {
        cur += 1;
        lastChangeTime = Time.time;
    }
    
    
}

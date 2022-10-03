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

[Serializable]
public class WeatherPrefab
{
    public GameObject none;
    public GameObject sunny;
    public GameObject rain;
    public GameObject fertilizerRain;
    public GameObject thunderStrom;
    public GameObject drought;
    public GameObject flood;
    public GameObject done;
}


public class Weather : Singleton<Weather>
{
    public WeatherType currentWeather => cur < 0 || cur >= seq.Count ? WeatherType.None : seq[cur];
    
    public List<WeatherType> seq = new List<WeatherType>();
    
    public WeatherLightColor lightColor = new WeatherLightColor();
    
    public WeatherPrefab weatherPrefab = new WeatherPrefab();
    
    public new Light light;
    
    public int cur = 0;
    
    public Timer timer = null;
    
    public float lastChangeTime = 0;
    
    public bool currentFinished = false;
    
    public bool started = false;
    
    public event Action onFinishedRound;
    
    public event Action onFailRound;
    
    public GameObject scene;
    
    public void Reset()
    {
        started = false;
        cur = -1;
        currentFinished = false;
        seq.Clear();
        GenerateSeq();
        timer?.Remove();
        this.light.color = GetLightColor(WeatherType.Sunny);
    }
    
    void Start()
    {
        PlayerState.instance.onBiomassChange += OnBiomassChange;
        Reset();
    }
    
    void OnDestroy()
    {
        PlayerState.instance.onBiomassChange -= OnBiomassChange;
    }
    
    void OnBiomassChange(int from, int to)
    {
        if(to - from > 0) currentFinished = true;
    }
    
    private void GenerateSeq()
    {
        seq.Add(WeatherType.Sunny);
        seq.Add(WeatherType.Sunny);
        seq.Add(WeatherType.Rain);
        seq.Add(WeatherType.FertilizerRain);
        
        var validCollection = new Dictionary<WeatherType, int>();
        validCollection.Add(WeatherType.Sunny, 20);
        validCollection.Add(WeatherType.Rain, 10);
        validCollection.Add(WeatherType.FertilizerRain, 10);
        validCollection.Add(WeatherType.ThunderStrom, 10);
        validCollection.Add(WeatherType.Flood, 5);
        validCollection.Add(WeatherType.Drought, 5);
        
        var last = WeatherType.None;
        
        for(int i = 5; i < 30; i++)  // 6 round per minute. 5min in total. 
        {
            var c = validCollection.Clone();
            c.Remove(last);     // no repeated weather.
            // no continuous flood or drought.
            // if(last == WeatherType.Flood) c.Remove(WeatherType.Drought);
            // if(last == WeatherType.Drought) c.Remove(WeatherType.Flood);
            
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
            
            validCollection[WeatherType.Flood] += 1;
            validCollection[WeatherType.Drought] += 1;
            validCollection[WeatherType.ThunderStrom] += 1;
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
        if(cur != -1)
        {
            if(!currentFinished) Boss.instance.Fail();
        }
        
        cur += 1;
        lastChangeTime = Time.time;
        currentFinished = false;
        
        var sourceColor = this.light.color;
        ProtaTweenManager.instance.New(TweenType.Transparency, this.light, (h, t) => {
            var targetColor = GetLightColor(this.currentWeather);
            this.light.color = (sourceColor, targetColor).Lerp(t);
        }).SetEase(TweenEase.quadInOut).SetFrom(0).SetTo(1).Start(1);
        
        var g = GetWeatherPrefab(this.currentWeather);
        if(g != null) GameObject.Instantiate(g);
        
        if(currentWeather == WeatherType.Done) Game.instance.completeGame = true;
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
    
    public GameObject GetWeatherPrefab(WeatherType type)
    {
        return type switch {
            WeatherType.Done => weatherPrefab.done,
            WeatherType.Rain => weatherPrefab.rain,
            WeatherType.Drought => weatherPrefab.drought,
            WeatherType.Sunny => weatherPrefab.sunny,
            WeatherType.Flood => weatherPrefab.flood,
            WeatherType.FertilizerRain => weatherPrefab.fertilizerRain,
            WeatherType.ThunderStrom => weatherPrefab.thunderStrom,
            _ => weatherPrefab.none,
        };
    }
    
}

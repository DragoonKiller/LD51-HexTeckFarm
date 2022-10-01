using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prota.Unity;
using Prota.Timer;
using System;


public class WeatherIcon : Singleton<WeatherIcon>
{
    public Sprite sunny;
    public Sprite rain;
    public Sprite fertilizerRain;
    public Sprite thunderStorm;
    public Sprite drought;
    public Sprite flood;
    public Sprite done;
    public Sprite none;
    
    public Sprite GetSprite(WeatherType type)
    {
        return type switch {
            WeatherType.Drought => drought,
            WeatherType.FertilizerRain => fertilizerRain,
            WeatherType.Sunny => sunny,
            WeatherType.Flood => flood,
            WeatherType.ThunderStrom => thunderStorm,
            WeatherType.Rain => rain,
            WeatherType.Done => done,
            _ => none,
        };
    }
}
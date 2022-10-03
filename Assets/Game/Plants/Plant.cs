using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Prota;
using Prota.Unity;

using Prota.Timer;
using Prota.Tween;


[Serializable]
public class GrowSpeed
{
    public float sunny;
    public float rain;
    public float thunderStorm;
    public float fertilizerRain;
    public float drought;
    public float flood;
    public float none;
}

[Serializable]
public class PlantAdaptionData
{
    public PlantAdaptionType sunny;
    public PlantAdaptionType rain;
    public PlantAdaptionType thunderStorm;
    public PlantAdaptionType fertilizerRain;
    public PlantAdaptionType drought;
    public PlantAdaptionType flood;
    public PlantAdaptionType none;
}


public abstract class Plant : MonoBehaviour
{
    public float ripeGrow = 10;
    
    public Block block;
    
    public PlantType type;
    
    [SerializeField] float _grow;
    public float grow
    {
        get => _grow;
        set => _grow = value.Clamp(0, ripeGrow);
    }
    
    public bool harvested = false;
    
    public float ripeRatio => grow / ripeGrow;
    
    public bool readyToHarvest => ripeRatio >= 1;
    
    protected Weather wt => Weather.instance;
    
    public GrowSpeed growSpeed = new GrowSpeed();
    
    public PlantAdaptionData adaption = new PlantAdaptionData();
    
    public new AudioSource audio;
    
    public virtual bool TryGrowStep()
    {
        var speed = GetGrowSpeed();
        grow += speed * Time.deltaTime;
        return speed > 0;
    }
    
    public abstract void UpdateDisplay(float from, float to);
    
    public virtual void Harvest()
    {
        harvested = true;
        block.ClearPlant();
        
        var tween = this.transform.TweenMove(this.transform.localPosition + Vector3.up * 2f, 0.8f);
        tween.SetEase(TweenEase.quadOut);
        var ntween = this.transform.TweenScale(Vector3.zero, 0.8f);
        ntween.SetEase(TweenEase.quadIn);
        Timer.New(1, () => GameObject.Destroy(this.gameObject));
        
        PlayerState.instance.biomass += Plants.instance.GetIncome(this.type);
        
        audio.PlayOneShot(Plants.instance.harvestAudio);
    }
    
    protected virtual void OnStart() { }
    
    
    void Start()
    {
        harvested = false;
        UpdateDisplay(0, 0);
        audio = this.gameObject.AddComponent<AudioSource>();
        audio.playOnAwake = false;
        audio.PlayOneShot(Plants.instance.plantAudio);
        OnStart();
    }
    
    
    void Update()
    {
        if(harvested) return;
        var from = grow;
        TryGrowStep();
        var to = grow;
        UpdateDisplay(from, to);
        if(readyToHarvest && PlayerState.instance.selection == this.block) Harvest();
    }
    
    public float GetGrowSpeed(WeatherType? weather = null)
    {
        if(weather == null) weather = wt.currentWeather;
        return weather.Value switch {
            WeatherType.Sunny => growSpeed.sunny,
            WeatherType.Rain => growSpeed.rain,
            WeatherType.Drought => growSpeed.drought,
            WeatherType.FertilizerRain => growSpeed.fertilizerRain,
            WeatherType.Flood => growSpeed.flood,
            WeatherType.ThunderStrom => growSpeed.thunderStorm,
            _ => 0,
        };
    }
}

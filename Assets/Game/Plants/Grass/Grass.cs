using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prota;
using Prota.Unity;

public class Grass : Plant
{
    
    public MeshRenderer rd;
    
    public Sprite[] grassLevels;
    
    void Awake()
    {
        (grassLevels.Length == 5).Assert();
    } 
    
    public override bool CanGrow()
    {
        if(wt.currentWeather == WeatherType.Sunny) return true;
        if(wt.currentWeather == WeatherType.Rain) return true;
        if(wt.currentWeather == WeatherType.ThunderStrom) return true;
        if(wt.currentWeather == WeatherType.FertilizerRain) return true;
        return false;
    }
    
    public override void GrowStep()
    {
        if(wt.currentWeather == WeatherType.FertilizerRain) grow += 2 * Time.deltaTime;     // double speed.
        else if(wt.currentWeather == WeatherType.Rain) grow += 0.5f * Time.deltaTime;     // half speed.
        else if(wt.currentWeather == WeatherType.ThunderStrom) grow += 0.5f * Time.deltaTime;     // half speed.
        else grow += Time.deltaTime;
    }
    
    public override void UpdateDisplay(float from, float to)
    {
        var ratio = to / ripeGrow;
        if(ratio <= 0.2f) rd.GetMaterialInstance().SetMainTex(grassLevels[0].texture);
        else if(ratio <= 0.4f) rd.GetMaterialInstance().SetMainTex(grassLevels[1].texture);
        else if(ratio <= 0.6f) rd.GetMaterialInstance().SetMainTex(grassLevels[2].texture);
        else if(ratio <= 0.8f) rd.GetMaterialInstance().SetMainTex(grassLevels[3].texture);
        else rd.GetMaterialInstance().SetMainTex(grassLevels[4].texture);
    }
    
}

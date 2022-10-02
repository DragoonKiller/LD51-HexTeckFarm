using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Prota;
using Prota.Unity;

using Prota.Timer;
using Prota.Tween;
using UnityEngine.UI;

[Serializable]
public enum PlantAdaptionType
{
    Negative = -1,
    Incompatible = 0,
    Positive = 1,
}

public class PlantAdaption : Singleton<PlantAdaption>
{
    public Plant grass;
    public Plant vine;
    public Plant tree;
    public Plant solarPanel;
    public Plant floriaWater;
    public Plant diamond;
    
    public Image sunny;
    public Image rain;
    public Image drought;
    public Image fertilizerRain;
    public Image flood;
    public Image thunderStorm;
    
    public Image positiveTemplate;
    public Image negativeTemplate;
    public Image incompatibleTemplate;
    
    public void SetData(PlantType type)
    {
        var adaptData = type switch {
            PlantType.Grass => grass.adaption,
            PlantType.Vine => vine.adaption,
            PlantType.Tree => tree.adaption,
            PlantType.SolarPanel => solarPanel.adaption,
            PlantType.FlorialWater => floriaWater.adaption,
            PlantType.Diamond => diamond.adaption,
            _ => new PlantAdaptionData(),
        };
        
        SetData(adaptData.sunny, sunny);
        SetData(adaptData.rain, rain);
        SetData(adaptData.drought, drought);
        SetData(adaptData.fertilizerRain, fertilizerRain);
        SetData(adaptData.flood, flood);
        SetData(adaptData.thunderStorm, thunderStorm);
        
    }
    
    void SetData(PlantAdaptionType type, Image dst)
    {
        var template = incompatibleTemplate;
        if(type == PlantAdaptionType.Positive) template = positiveTemplate;
        if(type == PlantAdaptionType.Negative) template = negativeTemplate;
        
        dst.sprite = template.sprite;
        dst.color = template.color;    
    }
    
}
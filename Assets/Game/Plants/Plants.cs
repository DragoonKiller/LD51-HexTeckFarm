using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Prota;
using Prota.Unity;

public enum PlantType
{
    None = 0,
    Grass,
    Vine,
    FlorialWater,
    SolarPanel,
    Tree,
    Diamond,
}

public class Plants : Singleton<Plants>
{
    public GameObject grass;
    public GameObject vine;
    public GameObject florialWater;
    public GameObject solarPanel;
    public GameObject tree;
    public GameObject diamond;
    public GameObject none;
    
    public GameObject universalRipeFx;
    
    public bool TryPlant(PlantType type)
    {
        var p = PlayerState.instance;
        if(p.selection == null) return false;
        if(p.selection.plant != null) return false;
         
        var plant = GameObject.Instantiate(type switch {
            PlantType.Grass => grass,
            PlantType.Vine => vine,
            PlantType.FlorialWater => florialWater,
            PlantType.SolarPanel => solarPanel,
            PlantType.Diamond => diamond,
            _ => none,
        }).GetComponent<Plant>();
        
        p.selection.Plant(plant);
        return true;
    }
    
}

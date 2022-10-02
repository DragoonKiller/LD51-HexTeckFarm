using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
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

[Serializable]
public class PlantsIconRecod
{
    public Sprite grass;
    public Sprite vine;
    public Sprite tree;
    public Sprite solarPanel;
    public Sprite florialWater;
    public Sprite diamond;
    public Sprite none;
}

[Serializable]
public class PlantsIncomeRecord
{
    public int grass;
    public int vine;
    public int tree;
    public int solarPanel;
    public int florialWater;
    public int diamond;
    public int none;
}

public class Plants : Singleton<Plants>
{
    public GameObject grass;
    public GameObject vine;
    public GameObject tree;
    public GameObject solarPanel;
    public GameObject florialWater;
    public GameObject diamond;
    public GameObject none;
    
    public PlantsIconRecod icons = new PlantsIconRecod();
    
    public PlantsIncomeRecord consume = new PlantsIncomeRecord();
    
    public PlantsIncomeRecord income = new PlantsIncomeRecord();
    
    public GameObject universalRipeFx;
    
    public bool MatchConsume(PlantType type)
    {
        var c = GetConsume(type);
        if(PlayerState.instance.biomass >= c) return true;
        return false;
    }
    
    public bool TryPlant(PlantType type)
    {
        var p = PlayerState.instance;
        if(p.selection == null) return false;
        if(p.selection.plant != null) return false;
        if(!MatchConsume(type)) return false;
         
        var plant = GameObject.Instantiate(type switch {
            PlantType.Grass => grass,
            PlantType.Vine => vine,
            PlantType.Tree => tree,
            PlantType.FlorialWater => florialWater,
            PlantType.SolarPanel => solarPanel,
            PlantType.Diamond => diamond,
            _ => none,
        }).GetComponent<Plant>();
        
        var consume = GetConsume(type);
        p.biomass -= consume;
        
        p.selection.Plant(plant);
        return true;
    }
    
    public int GetIncome(PlantType type)
    {
        return type switch {
            PlantType.Grass => income.grass,
            PlantType.Vine => income.vine,
            PlantType.Tree => income.tree,
            PlantType.FlorialWater => income.florialWater,
            PlantType.SolarPanel => income.solarPanel,
            PlantType.Diamond => income.diamond,
            _ => income.none,
        };
    }
    
    public int GetConsume(PlantType type)
    {
        return type switch {
            PlantType.Grass => consume.grass,
            PlantType.Vine => consume.vine,
            PlantType.Tree => consume.tree,
            PlantType.FlorialWater => consume.florialWater,
            PlantType.SolarPanel => consume.solarPanel,
            PlantType.Diamond => consume.diamond,
            _ => consume.none,
        };
    }
    
    public Sprite GetIcon(PlantType type)
    {
        return type switch {
            PlantType.Grass => icons.grass,
            PlantType.Vine => icons.vine,
            PlantType.Tree => icons.tree,
            PlantType.FlorialWater => icons.florialWater,
            PlantType.SolarPanel => icons.solarPanel,
            PlantType.Diamond => icons.diamond,
            _ => icons.none,
        };
    }
    
}

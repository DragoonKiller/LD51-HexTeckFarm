using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Prota;
using Prota.Unity;

public abstract class Plant : MonoBehaviour
{
    public float ripeGrow = 10;
    
    public Block block;
    
    public PlantType type;
    
    public float grow;
    
    public float ripeRatio => grow / ripeGrow;
    
    public bool readyToHarvest => ripeRatio >= 1;
    
    public bool shielded => block.shielded;
    
    protected Weather wt => Weather.instance;
    
    public virtual bool CanGrow()
    {
        return true;
    }
    
    public virtual void GrowStep()
    {
        grow += Time.deltaTime;
    }
    
    public abstract void UpdateDisplay(float from, float to);
    
    public virtual void Harvest()
    {
        // do some sfx.
        block.ClearPlant();
    }
    
    void Start()
    {
        UpdateDisplay(0, 0);
    }
    
    void Update()
    {
        var from = grow;
        if(CanGrow()) GrowStep();
        var to = grow;
        UpdateDisplay(from, to);
        if(readyToHarvest && PlayerState.instance.selection == this.block) Harvest();
    }
    
}

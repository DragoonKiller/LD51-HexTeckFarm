using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Prota;
using Prota.Unity;

using Prota.Timer;
using Prota.Tween;

public abstract class Plant : MonoBehaviour
{
    public float ripeGrow = 10;
    
    public Block block;
    
    public PlantType type;
    
    public float grow;
    
    public bool harvested = false;
    
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
        harvested = true;
        block.ClearPlant();
        
        var tween = this.transform.TweenMove(this.transform.localPosition + Vector3.up * 2f, 0.8f);
        tween.SetEase(TweenEase.quadOut);
        var ntween = this.transform.TweenScale(Vector3.zero, 0.8f);
        ntween.SetEase(TweenEase.quadIn);
        Timer.New(1, () => GameObject.Destroy(this.gameObject));
        
        PlayerState.instance.biomass += Plants.instance.GetIncome(this.type);
    }
    
    void Start()
    {
        harvested = false;
        UpdateDisplay(0, 0);
    }
    
    void Update()
    {
        if(harvested) return;
        var from = grow;
        if(CanGrow()) GrowStep();
        var to = grow;
        UpdateDisplay(from, to);
        if(readyToHarvest && PlayerState.instance.selection == this.block) Harvest();
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prota;
using Prota.Unity;
using Prota.Timer;
using Prota.Animation;

public class Diamond : Plant
{
    Timer timer;
    
    public SimpleAnimation anim;
    
    public SpriteRenderer rd;
    
    public override bool TryGrowStep()
    {
        // diamond won't grow.
        // instead, in non-flood weather, it has 6% chance to be devolved per sec.
        if(wt.currentWeather == WeatherType.Flood) return false;
        
        if(timer == null)
        {
            anim.gameObject.SetActive(false);
            rd.gameObject.SetActive(true);
            timer = Timer.New(1, true, () => {
                if(this.gameObject == null)
                {
                    ClearTimer();
                    return;
                }
                var hit = UnityEngine.Random.Range(0.0f, 1.0f) <= 0.06f;
                if(hit) DestroySelf();
            });
        }
        
        return false;
    }
    
    public override void UpdateDisplay(float from, float to)
    {
        
    }
    
    void ClearTimer()
    {
        timer?.Remove();
        timer = null;
    }
    
    void DestroySelf()
    {
        ClearTimer();
        block.ClearPlant();
        
        anim.gameObject.SetActive(true);
        anim.Restart();
        
        Timer.New(anim.duration - 0.1f, () => rd.gameObject.SetActive(false));
        Timer.New(anim.duration, () => GameObject.Destroy(this.gameObject));
    }
}
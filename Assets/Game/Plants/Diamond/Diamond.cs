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
    
    protected override void OnStart()
    {
        anim.gameObject.SetActive(false);
        rd.gameObject.SetActive(true);
    }
    
    public override bool TryGrowStep()
    {
        // diamond won't grow.
        // instead, in non-flood weather, it has 6% chance to be devolved per sec.
        if(wt.currentWeather == WeatherType.Flood) return false;
        
        if(block != null && timer == null)
        {
            timer = Timer.New(1, true, () => {
                $"Tiemr trigger { Time.frameCount }".Log();
                if(this == null)
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
        "Clear timer".Log();
        timer?.Remove();
        timer = null;
    }
    
    void DestroySelf()
    {
        (timer != null).Assert();
        ClearTimer();
        block.ClearPlant();
        
        anim.gameObject.SetActive(true);
        anim.Restart();
        
        
        $"Anim Start { Time.frameCount } :: { anim.duration }".Log();
        
        Timer.New(anim.duration - 0.1f, () => {
            $"t1 { anim.duration - 0.1f }".Log();
            rd.gameObject.SetActive(false);
        });
        Timer.New(anim.duration, () => {
            $"t2 { anim.duration }".Log();
            GameObject.Destroy(this.gameObject);
        });
    }
}
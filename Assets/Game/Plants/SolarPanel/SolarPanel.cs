using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prota;
using Prota.Unity;
using Prota.Animation;
using Prota.Timer;
using Prota.Tween;

public class SolarPanel : Plant
{
    public SpriteRenderer chargeComplete;
    
    public SimpleAnimation chargeAnim;
    
    public SpriteRenderer rd => chargeAnim.sprite;
    
    protected override void OnStart()
    {
        chargeAnim.Restart();
        chargeAnim.currentTime = UnityEngine.Random.Range(0f, chargeAnim.duration);
    }
    
    public override void UpdateDisplay(float from, float to)
    {
        chargeAnim.speedMultiply = GetGrowSpeed();
        
        if(ripeRatio >= 1)
        {
            chargeComplete.gameObject.SetActive(true);
            chargeAnim.gameObject.SetActive(false);
        }
        else if(to > from)
        {
            chargeComplete.gameObject.SetActive(false);
            chargeAnim.gameObject.SetActive(true);
        }
        else
        {
            chargeComplete.gameObject.SetActive(false);
            chargeAnim.gameObject.SetActive(false);
        }
    }

    public override void Harvest()
    {
        // solar panels cannot be retrieved.
        grow = 0;
        PlayerState.instance.biomass += 1;
    }
}
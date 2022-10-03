using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prota;
using Prota.Unity;

public class FlorialWater : Plant
{
    public SpriteRenderer content;
    
    public override void UpdateDisplay(float from, float to)
    {
        content.size = content.size.WithY(ripeRatio);
    }

    public override void Harvest()
    {
        // florial water don't retrieve.
        grow = 0;
        PlayerState.instance.biomass += 1;
        
        audio.PlayOneShot(Plants.instance.harvestAudio);
    }
}
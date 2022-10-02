using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prota;
using Prota.Unity;

public class Vine : Plant
{
    public MeshRenderer rd;
    
    public Sprite[] levels;
    
    public override void UpdateDisplay(float from, float to)
    {
        var ratio = to / ripeGrow;
        if(ratio <= 0.25f) rd.GetMaterialInstance().SetMainTex(levels[0].texture);
        else if(ratio < 0.5f) rd.GetMaterialInstance().SetMainTex(levels[1].texture);
        else if(ratio < 0.75f) rd.GetMaterialInstance().SetMainTex(levels[2].texture);
        else if(ratio < 1f) rd.GetMaterialInstance().SetMainTex(levels[3].texture);
        else rd.GetMaterialInstance().SetMainTex(levels[4].texture);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prota;
using Prota.Unity;

public class Grass : Plant
{
    
    public MeshRenderer rd;
    
    public Sprite[] grassLevels;
    
    void Awake()
    {
        (grassLevels.Length == 5).Assert();
    } 
    
    public override void UpdateDisplay(float from, float to)
    {
        var ratio = to / ripeGrow;
        if(ratio < 0.25f) rd.GetMaterialInstance().SetMainTex(grassLevels[0].texture);
        else if(ratio < 0.5f) rd.GetMaterialInstance().SetMainTex(grassLevels[1].texture);
        else if(ratio < 0.75f) rd.GetMaterialInstance().SetMainTex(grassLevels[2].texture);
        else if(ratio < 1f) rd.GetMaterialInstance().SetMainTex(grassLevels[3].texture);
        else rd.GetMaterialInstance().SetMainTex(grassLevels[4].texture);
    }
    
}

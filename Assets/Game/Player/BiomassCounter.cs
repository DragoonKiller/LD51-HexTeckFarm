using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prota.Unity;
using Prota;
using Prota.Timer;
using UnityEngine.InputSystem;
using System;

using UnityEngine.UI;
using Prota.Tween;

public class BiomassCounter : MonoBehaviour
{
    public Text biomassCounter;
    public Text addCounter;
    
    void Start()
    {
        biomassCounter.text = PlayerState.instance.biomass.ToString();
        PlayerState.instance.onBiomassChange += OnBiomassChange;
        addCounter.color = addCounter.color.WithA(0);
    }
    
    void OnDestroy()
    {
        if(PlayerState.instance != null) PlayerState.instance.onBiomassChange -= OnBiomassChange;
    }
    
    void OnBiomassChange(int from, int to)
    {
        biomassCounter.text = PlayerState.instance.biomass.ToString();
        
        ProtaTweenManager.instance.Remove(this, TweenType.Transparency);
        addCounter.text = (to - from > 0 ? "+" : "") + (to - from).ToString();
        ProtaTweenManager.instance.New(TweenType.Transparency, this, (h, t) => {
            addCounter.color = addCounter.color.WithA(1 - t);
        }).SetFrom(1).SetTo(0).Start(1.2f);
        
    }
    
}
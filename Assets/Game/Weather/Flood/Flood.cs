using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prota.Unity;
using Prota.Timer;
using System;
using Prota.Tween;
using System.Linq;
using Prota;

public class Flood : BaseWeatherBehaviour
{
    public GameObject fxTemplate;
    
    Timer timer;
    
    public Color fromColor;
    public Color toColor;
    
    protected override void Start()
    {
        base.Start();
        
        this.transform.SetParent(Weather.instance.scene.transform);
        
        
        timer = Timer.New(0.1f, true, () => {
            var pos = (Player.instance.min, Player.instance.max).Random();
            pos += Vector3.one * 0.05f;
            pos.z = (pos.z * 4).RoundToInt() / 4f;
            var g = GameObject.Instantiate(fxTemplate, pos, Quaternion.identity, this.transform);
            g.SetActive(true);
            
            var fromHeight = -3f;
            g.transform.position = g.transform.position.WithY(fromHeight);
            g.transform.TweenMoveY(0, 2).SetEase(TweenEase.circOut);
            Timer.New(2f, () => g.transform.TweenMoveY(fromHeight, 2).SetEase(TweenEase.circIn));
            
            var rd = g.GetComponentInChildren<SpriteRenderer>();
            rd.color = (fromColor, toColor).Random().WithA(0);
            rd.TweenColorA(1, 1).SetEase(TweenEase.circOut);
            Timer.New(3f, () => rd.TweenColorA(0, 1).SetEase(TweenEase.circIn));
            
            Timer.New(4f, () => GameObject.Destroy(g));
        });
        
        Timer.New(8f, false, () => {
            timer.Remove();
        });
        
    }
}
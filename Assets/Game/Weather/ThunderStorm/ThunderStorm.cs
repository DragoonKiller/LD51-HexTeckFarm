using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prota.Unity;
using Prota.Timer;
using System;
using Prota.Tween;
using System.Linq;
using Prota;

public class ThunderStorm : BaseWeatherBehaviour
{
    public GameObject dropTemplate;
    
    public GameObject thunderTemplate;
    
    Timer dropTimer;
    
    Timer thunderTimer;
    
    protected override void Start()
    {
        base.Start();
        
        this.transform.SetParent(Weather.instance.scene.transform);
        
        dropTimer = Timer.New(0.008f, true, () => {
            var pos = (Player.instance.min, Player.instance.max).Random();
            var g = GameObject.Instantiate(dropTemplate, pos, Quaternion.identity, this.transform);
            g.SetActive(true);
            g.transform.position = g.transform.position.WithY(5);
            var rd = g.GetComponent<SpriteRenderer>();
            rd.color = rd.color.WithA(0);
            g.transform.TweenMoveY(-0.5f, 0.7f);
            rd.TweenColorA(1, 0.5f);
            Timer.New(2f, () => GameObject.Destroy(g));
        });
        
        thunderTimer = Timer.New(0.5f, true, () => {
            
        });
        
        Timer.New(10f, false, () => {
            dropTimer.Remove();
            thunderTimer.Remove();
        });
    }
}
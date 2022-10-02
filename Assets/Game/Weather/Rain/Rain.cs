using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prota.Unity;
using Prota.Timer;
using System;
using Prota.Tween;
using System.Linq;
using Prota;

public class Rain : BaseWeatherBehaviour
{
    public GameObject template;
    
    Timer timer;
    
    protected override void Start()
    {
        base.Start();
        
        this.transform.SetParent(Weather.instance.scene.transform);
        
        timer = Timer.New(0.05f, true, () => {
            var pos = (Player.instance.min, Player.instance.max).Random();
            var g = GameObject.Instantiate(template, pos, Quaternion.identity, this.transform);
            g.SetActive(true);
            g.transform.position = g.transform.position.WithY(5);
            var rd = g.GetComponent<SpriteRenderer>();
            rd.color = rd.color.WithA(0);
            g.transform.TweenMoveY(-0.5f, 2);
            rd.TweenColorA(1, 2);
            Timer.New(2f, () => GameObject.Destroy(g));
        });
        
        Timer.New(10f, false, () => {
            timer.Remove();
        });
    }
}
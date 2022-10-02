using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prota.Unity;
using Prota.Timer;
using System;
using Prota.Tween;
using System.Linq;
using Prota;

public class Sunny : BaseWeatherBehaviour
{
    
    protected override void Start()
    {
        this.transform.SetParent(Weather.instance.scene.transform);
    }
}
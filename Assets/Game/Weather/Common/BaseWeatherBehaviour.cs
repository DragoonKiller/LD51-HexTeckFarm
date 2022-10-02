using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prota.Unity;
using Prota.Timer;
using System;
using Prota.Tween;
using System.Linq;
using Prota;

public abstract class BaseWeatherBehaviour : MonoBehaviour
{
    protected virtual void Start()
    {
        Timer.New(15f, false, () => {
            GameObject.Destroy(this.gameObject);
        });
    }
}

    
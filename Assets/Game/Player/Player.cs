using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prota.Unity;
using Prota;
using Prota.Timer;
using UnityEngine.InputSystem;
using System;

public class Player : Singleton<Player>
{
    public GameObject target;
    
    public Vector3 min;
    public Vector3 max;
    
    public float velocity = 1;
    
    // public Timer timer;
    
    public void Reset()
    {
        target.transform.position = 0.5f * (min + max);
        // timer?.Remove();
    }

    void FixedUpdate()
    {
        var targetDir = Vector3.zero;
        if(Keyboard.current.leftArrowKey.isPressed) targetDir += Vector3.left;
        if(Keyboard.current.rightArrowKey.isPressed) targetDir += Vector3.right;
        if(Keyboard.current.upArrowKey.isPressed) targetDir += Vector3.forward;
        if(Keyboard.current.downArrowKey.isPressed) targetDir += Vector3.back;
        targetDir = targetDir.normalized;
        
        var move = Time.fixedDeltaTime * velocity * targetDir;
        var rd = target.GetComponent<Rigidbody>();
        var targetPos = (rd.position + move).Clamp(min, max);
        rd.MovePosition(targetPos);
    }
    
    void Update()
    {
        // if(timer != null) return;
        
        bool success = false;
        var pl = Plants.Get();
        
        if(Keyboard.current.qKey.wasPressedThisFrame)
        {
            success = true;
            pl.TryPlant(PlantType.Grass);
        }
        else if(Keyboard.current.wKey.wasPressedThisFrame)
        {
            success = true;
            pl.TryPlant(PlantType.Vine);
        }
        else if(Keyboard.current.eKey.wasPressedThisFrame)
        {
            success = true;
            pl.TryPlant(PlantType.Tree);
        }
        else if(Keyboard.current.aKey.wasPressedThisFrame)
        {
            success = true;
            pl.TryPlant(PlantType.SolarPanel);
        }
        else if(Keyboard.current.sKey.wasPressedThisFrame)
        {
            success = true;
            pl.TryPlant(PlantType.FlorialWater);
        }
        else if(Keyboard.current.dKey.wasPressedThisFrame)
        {
            success = true;
            pl.TryPlant(PlantType.Diamond);
        }
        
        // if(success) timer = Timer.New(0.3f, () => timer = null);
    }
    
}
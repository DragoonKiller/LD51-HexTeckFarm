using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prota.Unity;
using Prota;
using Prota.Timer;
using UnityEngine.InputSystem;
using System;
using Prota.Animation;

public class Player : Singleton<Player>
{
    public GameObject target;
    
    public Vector3 min;
    public Vector3 max;
    
    public float velocity = 1;
    
    [SerializeField] SimpleAnimation anim;
    
    public SimpleAnimationAsset run;
    public SimpleAnimationAsset runUp;
    public SimpleAnimationAsset runDown;
    public SimpleAnimationAsset idle;
    public SimpleAnimationAsset idleUp;
    public SimpleAnimationAsset idleDown;
    
    [SerializeField] Vector3 lastDir = Vector3.zero;
    
    public void Reset()
    {
        target.transform.position = 0.5f * (min + max);
    }
    
    
    void FixedUpdate()
    {
        var targetDir = Vector3.zero;
        if(Keyboard.current.leftArrowKey.isPressed) targetDir += Vector3.left;
        if(Keyboard.current.rightArrowKey.isPressed) targetDir += Vector3.right;
        if(Keyboard.current.upArrowKey.isPressed) targetDir += Vector3.forward;
        if(Keyboard.current.downArrowKey.isPressed) targetDir += Vector3.back;
        targetDir = targetDir.normalized;
        
        PlayAnim(targetDir);
        
        var move = Time.fixedDeltaTime * velocity * targetDir;
        var rd = target.GetComponent<Rigidbody>();
        var targetPos = (rd.position + move).Clamp(min, max);
        rd.MovePosition(targetPos);
    }
    
    void PlayAnim(Vector3 targetDir)
    {
        
        void Play(SimpleAnimationAsset asset, bool mirror)
        {
            anim.Play(asset);
            anim.mirror = mirror;
        }
        
        if(targetDir == Vector3.zero)
        {
            if(lastDir.x != 0) Play(idle, lastDir.x < 0);
            else
            {
                if(lastDir.z > 0) Play(idleUp, false);
                else Play(idleDown, false);
            }
        }
        else
        {
            lastDir = targetDir;
            
            if(targetDir.x != 0) Play(run, lastDir.x > 0);
            else
            {
                if(targetDir.z > 0) Play(runUp, false);
                else Play(runDown, false);
            }
        }
        
    }
    
    void Update()
    {
        if(Keyboard.current.equalsKey.wasPressedThisFrame
        || Keyboard.current.numpadPlusKey.wasPressedThisFrame)
            AudioListener.volume *= 2f;
            
        if(Keyboard.current.minusKey.wasPressedThisFrame
        || Keyboard.current.numpadMinusKey.wasPressedThisFrame)
            AudioListener.volume *= 0.5f;
            
        if(Keyboard.current.digit0Key.wasPressedThisFrame
        || Keyboard.current.numpad0Key.wasPressedThisFrame)
            AudioListener.volume = 0.5f;
        
        if(Weather.instance.currentWeather == WeatherType.Done) return;
        if(Weather.instance.currentWeather == WeatherType.Flood) return;
        
        var pl = Plants.instance;
        var ad = PlantAdaption.instance;
        
        if(Keyboard.current.qKey.wasPressedThisFrame)
        {
            pl.TryPlant(PlantType.Grass);
            ad.SetData(PlantType.Grass);
        }
        else if(Keyboard.current.wKey.wasPressedThisFrame)
        {
            pl.TryPlant(PlantType.Vine);
            ad.SetData(PlantType.Vine);
        }
        else if(Keyboard.current.eKey.wasPressedThisFrame)
        {
            pl.TryPlant(PlantType.Tree);
            ad.SetData(PlantType.Tree);
        }
        else if(Keyboard.current.aKey.wasPressedThisFrame)
        {
            pl.TryPlant(PlantType.SolarPanel);
            ad.SetData(PlantType.SolarPanel);
        }
        else if(Keyboard.current.sKey.wasPressedThisFrame)
        {
            pl.TryPlant(PlantType.FlorialWater);
            ad.SetData(PlantType.FlorialWater);
        }
        else if(Keyboard.current.dKey.wasPressedThisFrame)
        {
            pl.TryPlant(PlantType.Diamond);
            ad.SetData(PlantType.Diamond);
        }
    }
    
}
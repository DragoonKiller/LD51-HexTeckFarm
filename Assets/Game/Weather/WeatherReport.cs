using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prota;
using System;
using UnityEngine.UI;
using Prota.Tween;
using Prota.Unity;

public class WeatherReport : Singleton<WeatherReport>
{
    public List<Image> instances = new List<Image>();
    
    public Transform tweenRoot;
    
    public Vector3 stayPos;
    
    public int recordCur;
    
    Weather wt => Weather.Get();
    
    
    void Awake()
    {
        stayPos = tweenRoot.localPosition;
        // stayPos.ToString().Log();
    }
    
    void Start()
    {
        Reset();
        SetCurrentState();
    }
    
    public void Reset()
    {
        tweenRoot.localPosition = stayPos.WithY(stayPos.y + 400);
        this.recordCur = -1;
    }
    
    
    
    void Update()
    {
        if(wt.cur != this.recordCur)
        {
            if(this.recordCur == -1) tweenRoot.TweenMoveY(stayPos.y, 0.5f).SetEase(TweenEase.quadOut);
            this.recordCur = wt.cur;
            SetCurrentState();
            PlayAnim();
        }
    }
    
    void PlayAnim()
    {
        var pos = new List<float>();
        for(int i = 0; i < 9; i++) pos.Add(instances[i].transform.localPosition.x);
        for(int i = 0; i < 8; i++)
        {
            var tr = instances[i].transform;
            tr.localPosition = tr.localPosition.WithX(pos[i + 1]);
            tr.TweenMoveX(pos[i], 0.5f).SetEase(TweenEase.quadOut);
        }
    }
    
    void SetCurrentState()
    {
        if(wt.cur == -1) return; // not yet started, but reset.
        var seq = wt.seq;
        for(int i = 0; i < 8; i++)
        {
            var w = i + wt.cur < seq.Count ? seq[i + wt.cur] : WeatherType.None;
            var sprite = WeatherIcon.instance.GetSprite(w);
            instances[i].sprite = sprite;
        }
    }
    
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prota;
using System;
using UnityEngine.UI;
using Prota.Tweening;

public class WeatherReport : MonoBehaviour
{
    public List<Image> instances = new List<Image>();
    
    Weather wt => Weather.Get();
    
    Vector3 stayPos;
    
    int recordCur;
    
    void Awake()
    {
        stayPos = this.transform.localPosition;
    }
    
    void Start()
    {
        Reset();
        SetCurrentState();
    }
    
    void Reset()
    {
        this.transform.localPosition = stayPos.WithY(stayPos.y - 400);
        this.transform.TweenMoveY(stayPos.y, 0.5f).SetEase(TweenEase.quadOut);
        this.recordCur = -1;
    }
    
    
    
    void Update()
    {
        if(wt.cur != this.recordCur)
        {
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
            tr.position = tr.position.WithX(pos[i + 1]);
            tr.TweenMoveX(pos[i], 0.5f).SetEase(TweenEase.quadOut);
        }
    }
    
    void SetCurrentState()
    {
        var seq = wt.seq;
        for(int i = 0; i < 8; i++)
        {
            var w = i + wt.cur < seq.Count ? seq[i + wt.cur] : WeatherType.None;
            var sprite = WeatherIcon.Get().GetSprite(w);
            instances[i].sprite = sprite;
        }
    }
    
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prota.Unity;
using Prota.Timer;
using System;
using Prota.Tween;
using System.Linq;
using Prota;
using UnityEngine.UI;

public class Boss : Singleton<Boss>
{
    public GameObject bg;
    
    public readonly List<Action> actions = new List<Action>();
    public bool inuse;
    
    public int failTimes;
    
    public GameObject[] talks;
    public GameObject[] warns;
    
    void Start()
    {
        
    }
    
    public void Fail()
    {
        failTimes += 1;
        
        if(failTimes <= 3)
        {
            actions.Add(() => {
                inuse = true;
                warns[failTimes - 1].gameObject.SetActive(true);
                bg.gameObject.SetActive(true);
                Timer.New(6, () => {
                    warns[failTimes - 1].gameObject.SetActive(false);
                    bg.gameObject.SetActive(false);
                    inuse = false;
                });
            });
        }
    }
    
    public void Reset()
    {
        failTimes = 0;
        actions.Clear();
        
        for(int i = 0; i < 3; i++)
        {
            var g = i;
            actions.Add(() => {
                inuse = true;
                talks[g].gameObject.SetActive(true);
                bg.gameObject.SetActive(true);
                Timer.New(3, () => {
                    talks[g].gameObject.SetActive(false);
                    bg.gameObject.SetActive(false);
                    inuse = false;
                });
            });
        }
    }
    
    void Update()
    {
        if(!inuse) DoAction();
    }
    
    void DoAction()
    {
        if(actions.Count == 0) return;
        var x = actions.First();
        actions.RemoveAt(0);
        x();
    }
    
}
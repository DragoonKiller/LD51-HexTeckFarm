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
            Timer.New(2f, () => g.Destroy());
        });
        
        thunderTimer = Timer.New(0.5f, true, () => {
            var coord = (Vector2Int.zero, Ground.instance.size).Random();
            
            // 50% chance to detroy a tree.
            foreach(var bk in Ground.instance.blocks) if(bk?.plant?.type == PlantType.Tree)
            {
                if((0, 2).Random() == 0)
                {
                    coord = bk.coord;
                    break;
                }
            }
            
            var pos = Block.Pos(coord);
            var g = GameObject.Instantiate(thunderTemplate, pos, Quaternion.identity, this.transform);
            g.SetActive(true);
            g.transform.position = pos;
            var rd = g.GetComponentInChildren<SpriteRenderer>();
            rd.transform.localScale = rd.transform.localScale.WithX((0, 2).Random() * 2 - 1);
            rd.color = rd.color.WithA(1);
            rd.TweenColorA(0, 1f);
            Timer.New(2f, () => g.Destroy());
            
            var b = Ground.instance.blocks[coord.x, coord.y];
            if(b != null && b.plant != null)
            {
                if(b.plant.type != PlantType.Diamond
                && b.plant.type != PlantType.FlorialWater
                && b.plant.type != PlantType.SolarPanel)
                {
                    Destroy(b.plant.gameObject);        // remove it.
                    b.ClearPlant();
                }
            }
        });
        
        Timer.New(10f, false, () => {
            dropTimer.Remove();
            thunderTimer.Remove();
        });
    }
    
    void OnDestroy()
    {
        dropTimer.Remove();
        dropTimer = null;
        thunderTimer.Remove();
        thunderTimer = null;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prota.Unity;
using Prota.Timer;
using System;
using Prota;
using Prota.Tween;

[ExecuteAlways]
public class Block : MonoBehaviour
{
    public Vector2Int coord;
    
    public Plant plant;
    
    public GameObject model;
    
    public GameObject particleTemplate;
    
    public Timer particleTimer;
    
    void Start()
    {
        model.transform.rotation = Quaternion.Euler(90, 90 * UnityEngine.Random.Range(0, 4), 0);
        
        
        particleTimer = Timer.New(0.3f, true, () => {
            
            if(plant == null) return;
            if(plant.GetGrowSpeed() <= 0) return;
            
            var min = Pos(coord) - new Vector3(1, 0, 1);
            var max = Pos(coord) + new Vector3(1, 0, 1);
            var pos = (min, max).Random();
            var g = GameObject.Instantiate(particleTemplate, pos, Quaternion.identity, this.transform);
            g.SetActive(true);
            g.transform.position = g.transform.position.WithY(-0.5f);
            g.transform.TweenMoveY(1, 2);
            g.GetComponent<SpriteRenderer>().TweenColorA(0, 2);
            Timer.New(2f, () => GameObject.Destroy(g));
        });
    }
    
    void Update()
    {
        this.transform.position = Pos(coord);
    }
    
    public void Plant(Plant plant)
    {
        ClearPlant();
        this.plant = plant;
        plant.transform.SetParent(this.transform, false);
        plant.transform.localPosition = Vector3.zero;
        plant.name = "Plant:" + plant.type;
        plant.block = this;
    }
    
    public void ClearPlant()
    {
        if(this.plant == null) return;
        plant.block = null;
        this.plant = null;
    }
    
    
    public static Vector3 Pos(Vector2Int coord)
    {
        return new Vector3(coord.x * 2 + 2f, 0f, coord.y * 2 + 2f);
    }
    
    public static Vector2Int Coord(Vector3 pos)
    {
        return new Vector2Int(((pos.x - 2f) / 2).RoundToInt(), ((pos.z - 2f) / 2).RoundToInt());
    }
}
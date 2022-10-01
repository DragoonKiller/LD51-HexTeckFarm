using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prota.Unity;
using Prota.Timer;
using System;
using Prota;

[ExecuteAlways]
public class Block : MonoBehaviour
{
    public Vector2Int coord;
    
    public Plant plant;
    
    public GameObject model;
    
    public GameObject shieldTemplate;
    
    [Header("Runtime")]
    [SerializeField] GameObject shield;
    
    public bool shielded
    {
        get => shield = null;
        set
        {
            if(value)
            {
                if(shield != null) return;
                shield = GameObject.Instantiate(shieldTemplate, this.transform, false); 
                shield.transform.localPosition = Vector3.zero;
            }
            else
            {
                if(shield = null) return;
                GameObject.Destroy(shield);
                shield = null;
            }
        }
    }
    
    void Start()
    {
        model.transform.rotation = Quaternion.Euler(90, 90 * UnityEngine.Random.Range(0, 4), 0);
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
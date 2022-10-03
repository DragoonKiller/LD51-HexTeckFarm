using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prota.Unity;
using Prota.Timer;
using System;
using Prota;

public class Ground : Singleton<Ground>
{
    public Transform groundRoot;
    
    public Block blockTemplate;
    
    [Header("Runtime")]
    
    public Block[,] blocks = new Block[9, 9];
    
    public Vector2Int size => new Vector2Int(blocks.GetLength(0), blocks.GetLength(1));
    
    void Start()
    {
        blocks.Fill((_, _) => null); 
        
        List<Vector2Int> coords = new List<Vector2Int>();
        for(int i = 0; i < blocks.GetLength(0); i++)
            for(int j = 0; j < blocks.GetLength(1); j++)
                coords.Add(new Vector2Int(i, j));
        
        coords.Shuffle();
        coords.Shrink(25);
        
        foreach(var c in coords)
        {
            var x = blocks[c.x, c.y] = GameObject.Instantiate(blockTemplate, groundRoot).GetComponent<Block>();
            x.coord = c;
        }
    }
    
    public void Reset()
    {
        foreach(var b in blocks) if(b != null)
        {
            if(b.plant != null) b.plant.gameObject.Destroy();
            b.gameObject.Destroy();
        }
        Start();
    }
    
}
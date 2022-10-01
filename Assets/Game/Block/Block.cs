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
    
    void Update()
    {
        this.transform.position = Pos(coord);
    }
    
    public static Vector3 Pos(Vector2Int coord)
    {
        return new Vector3(coord.x * 2 + 1.5f, 0f, coord.y * 2 + 1.5f);
    }
    
    public static Vector2Int Coord(Vector3 pos)
    {
        return new Vector2Int(((pos.x - 1.5f) / 2).RoundToInt(), ((pos.z - 1.5f) / 2).RoundToInt());
    }
}
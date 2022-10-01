using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prota.Unity;
using UnityEngine.InputSystem;

public class PlayerState : Singleton<PlayerState>
{
    public int money;
    
    public Player player => Player.instance;
    
    public Vector2Int standingCoord => Block.Coord(player.target.transform.position);
    
    public GameObject selectFxTemplate;
    
    [Header("Runtime")]
    
    public GameObject selectFx;
    
    public Block selection
    {
        get
        {
            var gr = Ground.Get();
            var c = standingCoord;
            if(0 <= c.x && c.x < gr.blocks.GetLength(0) && 0 <= c.y && c.y < gr.blocks.GetLength(1))
                return gr.blocks[c.x, c.y];
            
            return null;
        }
    }
    
    public void Reset()
    {
        money = 0;
    }
    
    void Start()
    {
        selectFx = Instantiate(selectFxTemplate, this.transform, false);
        selectFx.SetActive(false);
    }
    
    void Update()
    {
        selectFx.SetActive(selection != null);
        if(selection == null) return;
        selectFx.transform.SetParent(selection.transform, true);
        selectFx.transform.localPosition = Vector3.zero;
    }
}
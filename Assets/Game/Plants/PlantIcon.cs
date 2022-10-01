using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using Prota;
using Prota.Unity;

using Prota.Timer;
using Prota.Tweening;


public class PlantIcon : MonoBehaviour
{
    public PlantType type;
    
    public Text consumeText;
    
    public Image mask;
    
    public Color enoughColor;
    
    public Color notEnoughColor;
    
    int consume => Plants.instance.GetConsume(type);
    
    void Start()
    {
        consumeText.text = consume.ToString();
    }
    
    void Update()
    {
        var p = PlayerState.instance;
        var enough = p.biomass >= consume;
        mask.gameObject.SetActive(p.selection != null || !enough);
        
        consumeText.color = enough ? enoughColor : notEnoughColor;
    }
    
}
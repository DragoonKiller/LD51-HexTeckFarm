using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Prota;
using Prota.Unity;
using Prota.VisualEffect;

[RequireComponent(typeof(RectangleDeformation))]
public class Waving : MonoBehaviour
{
    RectangleDeformation r => this.GetComponent<RectangleDeformation>();
    
    public float offset;
    
    public float phaseDiff;
    
    public float amplitude = 0.05f;
    
    public float peroid = 4f;
    
    void Start()
    {
        offset = UnityEngine.Random.Range(0.0f, 4.0f);
        phaseDiff = UnityEngine.Random.Range(0f, 0.2f);
    }
    
    void Update()
    {
        r.coordTopLeft.x = Mathf.Sin(offset + Time.time * (Mathf.PI * 2) / peroid) * amplitude;
        r.coordTopRight.x = 1 + Mathf.Sin(offset + phaseDiff + Time.time * (Mathf.PI * 2) / peroid) * amplitude;
    }
}
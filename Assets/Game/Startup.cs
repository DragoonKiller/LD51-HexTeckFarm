using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Startup : MonoBehaviour
{
    void Awake()
    {
        GameObject.DontDestroyOnLoad(this.gameObject);
    }
    
}

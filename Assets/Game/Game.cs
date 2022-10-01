using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prota.Unity;
using UnityEngine.InputSystem;
using Prota;

public class Game : Singleton<Game>
{
    public GameObject menuObj;
    
    int _needPause;
    public int needPause
    {
        get => _needPause;
        set
        {
            _needPause = value;
            if(_needPause == 0)
            {
                Time.timeScale = 1;
                menuObj.SetActive(false);
            }
            else
            {
                Time.timeScale = 0;
                menuObj.SetActive(true);
            }
        }
    }
    
    public bool activePause;
    
    void Awake()
    {
        GameObject.DontDestroyOnLoad(this.gameObject);
    }
    
    public void Reset()
    {
        Weather.instance.Restart();
        Player.instance.Restart();
        WeatherReport.instance.Reset();
    }
    
    
    void Update()
    {
        if(Keyboard.current.hKey.wasPressedThisFrame)
        {
            Reset();
            return;
        }
        
        if(Keyboard.current.pKey.wasPressedThisFrame || Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if(activePause)
            {
                activePause = false;
                needPause -= 1;
            }
            else
            {
                activePause = true;
                needPause += 1;
            }
        }
        
    }
    
}

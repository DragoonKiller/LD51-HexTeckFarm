using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prota.Unity;
using UnityEngine.InputSystem;
using Prota;

public class Game : Singleton<Game>
{
    public GameObject menuObj;
    
    public GameObject menuMask;
    
    public GameObject win;
    
    public GameObject fail;
    
    public bool completeGame;
    
    public bool activePause;
    
    void Awake()
    {
        GameObject.DontDestroyOnLoad(this.gameObject);
    }
    
    public void Reset()
    {
        PlayerState.instance.Reset();
        Player.instance.Reset();
        Weather.instance.Reset();
        WeatherReport.instance.Reset();
        Ground.instance.Reset();
        Boss.instance.Reset();
    }
    
    void Start()
    {
        AudioListener.volume = 0.3f;
        Time.timeScale = 1;
        activePause = false;
        Reset();
    }
    
    void Update()
    {
        if(completeGame || Boss.instance.failTimes > 3)
        {
            activePause = true;         // force pause.
            Time.timeScale = 0;
            if(Boss.instance.failTimes > 3)
            {
                Activate(3);
            }
            else
            {
                Activate(2);
            }
        }
        else
        {
            if(Keyboard.current.pKey.wasPressedThisFrame || Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                if(activePause)
                {
                    activePause = false;
                    Time.timeScale = 1;
                    Activate(0);
                }
                else
                {
                    activePause = true;
                    Time.timeScale = 0;
                    Activate(1);
                }
                return;
            }
        }
        
        if(activePause && Keyboard.current.hKey.isPressed)
        {
            Reset();
            return;
        }
        
        void Activate(int type)
        {
            menuMask.SetActive(type != 0);
            menuObj.SetActive(type == 1);
            win.SetActive(type == 2);
            fail.SetActive(type == 3);
        }
        
    }
    
}

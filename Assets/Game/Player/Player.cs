using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prota;
using UnityEngine.InputSystem;

public class Player : Singleton<Player>
{
    public GameObject target;
    
    public Vector3 min;
    public Vector3 max;
    
    public float velocity = 1; 
    
    void FixedUpdate()
    {
        var targetDir = Vector3.zero;
        if(Keyboard.current.leftArrowKey.isPressed) targetDir += Vector3.left;
        if(Keyboard.current.rightArrowKey.isPressed) targetDir += Vector3.right;
        if(Keyboard.current.upArrowKey.isPressed) targetDir += Vector3.forward;
        if(Keyboard.current.downArrowKey.isPressed) targetDir += Vector3.back;
        targetDir = targetDir.normalized;
        
        var move = Time.fixedDeltaTime * velocity * targetDir;
        var rd = target.GetComponent<Rigidbody>();
        var targetPos = (rd.position + move).Clamp(min, max);
        rd.MovePosition(targetPos);
    }
    
}
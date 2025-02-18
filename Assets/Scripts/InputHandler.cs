using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    void Update()
    {
        Vector2 inputvector = Vector2.zero;
        
        inputvector.x = Input.GetAxisRaw("Horizontal");
        inputvector.y = Input.GetAxisRaw("Vertical");
        
        GameManager.ActiveCar()?.UpdateInputs(inputvector);
    }
}

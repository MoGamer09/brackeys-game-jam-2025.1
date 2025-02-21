using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public Action OnInputMade = () => {};
    
    void Update()
    {
        Vector2 inputvector = Vector2.zero;
        
        inputvector.x = Input.GetAxisRaw("Horizontal");
        inputvector.y = Input.GetAxisRaw("Vertical");

        if (inputvector.x != 0 || inputvector.y != 0)
        {
            OnInputMade();
        }
        
        GameManager.ActiveCar()?.UpdateInputs(inputvector);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public IInputReceiver CurrentInputReceiver;

    void Update()
    {
        Vector2 inputvector = Vector2.zero;
        
        inputvector.x = Input.GetAxis("Horizontal");
        inputvector.y = Input.GetAxis("Vertical");
        
        GameManager.ActiveCar()?.UpdateInputs(inputvector);
    }

    private void SetCarController()
    {
        CurrentInputReceiver = GameManager.ActiveCar();
    }
}

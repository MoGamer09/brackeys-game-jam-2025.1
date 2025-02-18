using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public IInputReceiver CurrentInputReceiver;

    private void Awake()
    {
        CurrentInputReceiver = FindObjectOfType<CarController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputvector = Vector2.zero;
        
        inputvector.x = Input.GetAxis("Horizontal");
        inputvector.y = Input.GetAxis("Vertical");
        
        CurrentInputReceiver.UpdateInputs(inputvector);
    }
}

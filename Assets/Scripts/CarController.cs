using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour, IInputReceiver
{
    public float accelerationSpeed = 30.0f;
    public float turnSpeed = 3.5f;
    public float driftFactor = 0.80f;
    
    private float _accelerationInput = 0.0f;
    private float _turnInput = 0.0f;
    
    private float _rotationAngle = 0.0f;
    
    private Rigidbody2D _rb;
    
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void UpdateInputs(Vector2 inputs)
    {
        _turnInput = inputs.x;
        _accelerationInput = inputs.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ApplyEngineForce();
        KillOrthogonalVelocity();
        ApplySteering();
    }

    private void KillOrthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(_rb.linearVelocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(_rb.linearVelocity, transform.right);
        
        _rb.linearVelocity = forwardVelocity + rightVelocity * driftFactor;
    }

    private void ApplySteering()
    {
        _rotationAngle -= _turnInput * turnSpeed;
        _rb.MoveRotation(_rotationAngle);
    }

    private void ApplyEngineForce()
    {
        Vector2 engineForce = transform.up * (_accelerationInput * accelerationSpeed);
        _rb.AddForce(engineForce, ForceMode2D.Force);
    }
}

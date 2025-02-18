using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CarPhysics))]
public class CarController : MonoBehaviour, IInputReceiver
{
    public float accelerationSpeed = 30.0f;
    public float turnSpeed = 3.5f;
    public float maxSpeed = 10.0f;
    
    private float _accelerationInput = 0.0f;
    private float _turnInput = 0.0f;
    
    private float _rotationAngle = 0.0f;
    private float _velocityVsUp = 0.0f;
    
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
        ApplySteering();
    }

    private void ApplySteering()
    {
        _rotationAngle -= Mathf.Sign(_turnInput) * Mathf.Log(Mathf.Abs(_turnInput) * 5.0f + 1.0f, 2) * turnSpeed;
        _rb.MoveRotation(_rotationAngle);
    }

    private void ApplyEngineForce()
    {
        _velocityVsUp = Vector2.Dot(transform.up, _rb.linearVelocity);
        
        //Limit velocity
        if(_velocityVsUp > maxSpeed && _accelerationInput > 0)
            return;
        
        //Limit backwards velocity
        if(_velocityVsUp < -maxSpeed/4 && _accelerationInput < 0)
            return;
        
        //Limit sideways speed
        if(_rb.linearVelocity.sqrMagnitude > maxSpeed*maxSpeed && _accelerationInput > 0)
            return;
        
        if (_accelerationInput == 0)
        {
            _rb.linearDamping = Mathf.Lerp(_rb.linearDamping, 3.0f, Time.fixedDeltaTime * accelerationSpeed * 3);
        }
        else
        {
            _rb.linearDamping = 0;
        }
        
        Vector2 engineForce = transform.up * (_accelerationInput * accelerationSpeed);
        _rb.AddForce(engineForce, ForceMode2D.Force);
    }
}

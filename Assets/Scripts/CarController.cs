using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CarPhysics))]
public class CarController : MonoBehaviour, IInputReceiver
{
    public float accelerationSpeed = 30.0f;
    public float turnSpeed = 3.5f;
    public float maxSpeed = 10.0f;
    public float minTurningSpeed = 0.5f;

    private float _accelerationInput = 0.0f;
    private float _turnInput = 0.0f;
    
    private float _rotationAngle = 0.0f;
    private float _velocityVsUp = 0.0f;
    
    private Rigidbody2D _rb;

    private bool _playerIsAlive;
    private bool _followPath;
    private RecordEntry[] _path = Array.Empty<RecordEntry>();
    [HideInInspector]
    public int pathSize;
    private bool _exploded;
    [HideInInspector]
    public int priority;

    private Func<int> PathIndex;

    [SerializeField]
    private GameObject[] _adherents;
    
    private IndicatorManager _indicatorManager;
    
    private List<TrailRenderer> _tiremarks = new List<TrailRenderer>();

    void Start()
    {
        _exploded = false;
    }
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _followPath = false;
        _playerIsAlive = true;
        _rotationAngle = transform.eulerAngles.z;
        _indicatorManager = GetComponentInChildren<IndicatorManager>();
        _indicatorManager.ShowIndicator();
        _tiremarks.AddRange(GetComponentsInChildren<TrailRenderer>());
        foreach (var adherent in _adherents)
        {
            _tiremarks.AddRange(adherent.GetComponentsInChildren<TrailRenderer>());
        }
    }

    public void UpdateInputs(Vector2 inputs)
    {
        _turnInput = inputs.x;
        _accelerationInput = inputs.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_followPath)
            DriveByPath();
        else
            DriveByPlayer();
    }

    public void SetPath(RecordEntry[] path, Func<int> pathIndexCallback)
    {
        _path = path;
        _followPath = true;
        PathIndex = pathIndexCallback;
        
        ResetPath();
    }

    public bool IsDrivenByPlayer()
    {
        return !_followPath;
    }

    public bool IsFollowingPath()
    {
        return _followPath;
    }

    public RecordEntry PathEntry()
    {
        var adherentEntries = new RecordEntry[_adherents.Length];
        for (var i = 0; i < _adherents.Length; i++)
        {
            adherentEntries[i].position = _adherents[i].transform.position;
            adherentEntries[i].rotation = _adherents[i].transform.rotation;
        }

        return new RecordEntry()
        {
            position = transform.position,
            rotation = transform.rotation,
            adherents = adherentEntries,
        };
    }

    public void KillPlayer()
    {
        _playerIsAlive = false;
        _rb.linearVelocity = Vector2.zero;
        _rb.angularVelocity = 0.7f;
    }

    public void Explode(Collision2D other)
    {
        if (_exploded
        || IsDrivenByPlayer()
        || other.gameObject.GetComponent<ExplosionTrigger>().GetCarController().priority < priority)
            return;
        
        _exploded = true;
        if (IsFollowingPath())
        {
            pathSize = PathIndex() + 1;
        }
    }

    private void DriveByPath()
    {
        if (PathIndex() > pathSize)
        {
            _rb.linearDamping = 3.0f;
            _rb.angularDamping = 3.0f;
            return;
        }
        if (pathSize == 0) return;
        var pathIndex = Math.Min(PathIndex(), pathSize - 1);
        transform.position = _path[pathIndex].position;
        transform.rotation = _path[pathIndex].rotation;

        for (var i = 0; i < _adherents.Length; i++)
        {
            _adherents[i].transform.position = _path[pathIndex].adherents[i].position;
            _adherents[i].transform.rotation = _path[pathIndex].adherents[i].rotation;
        }
        
        if (PathIndex() <= 1)
        {
            RemoveTireMarks();
        }
    }

    public void RemoveTireMarks()
    {
        foreach (var tiremark in _tiremarks)
        {
            tiremark.Clear();
        }
    }

    public void ResetPath()
    {
        pathSize = _path.Length;
        _exploded = false;
    }

    private void DriveByPlayer()
    {
        if (!_indicatorManager.IsHidden())
        {
            if (_accelerationInput != 0)
            {
                _indicatorManager.HideIndicator();
            }
        }
        
        if (!_playerIsAlive) return;
        
        ApplySteering();
        ApplyEngineForce();
    }

    private void ApplySteering()
    {
        if (_rb.linearVelocity.magnitude < minTurningSpeed)
            return;
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
    
    public static T GetComponent<T>(GameObject carParent)
    {
        var t = carParent.GetComponent<T>() ?? carParent.GetComponentInChildren<T>();
        if (t == null)
            throw new UnityException("No CarController found");
        
        return t;
    }
    public static T[] GetAllComponents<T>(GameObject carParent)
    {
        var ts = new List<T>();
        ts.AddRange(carParent.GetComponents<T>());
        ts.AddRange(carParent.GetComponentsInChildren<T>());
        
        return ts.ToArray();
    }
}

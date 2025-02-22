using System;
using UnityEditor.Searcher;
using UnityEngine;

public class WaypointBehaviour : MonoBehaviour
{
    private Action _onWaypointReached;
    private WaypointBehaviour _nextWaypoint;

    private bool _deactivated = false;
    private bool _canBeReached = false;
    private IndicatorManager _indicatorManager;
    private SpriteRenderer _spriteRenderer;

    public Color finalWaypointColor;
    public Color chainWaypointColor;

    private void Awake()
    {
        _indicatorManager = GetComponentInChildren<IndicatorManager>();
        _deactivated = false;
        _indicatorManager.ShowIndicator();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        UpdateColor();
    }

    private void UpdateColor()
    {
        if (_nextWaypoint != null)
        {
            _spriteRenderer.color = chainWaypointColor;
            _indicatorManager.color = chainWaypointColor;   
        }
        else
        {
            _spriteRenderer.color = finalWaypointColor;
            _indicatorManager.color = finalWaypointColor;
        }
    }

    public void Init(Action onFinalWaypointReached, GameObject[] nextWaypoints)
    {
        _canBeReached = true;
        
        if (nextWaypoints.Length == 0) return;
        var waypoint = this;
        for (var i = 1; i < nextWaypoints.Length; i++)
        {
            waypoint._onWaypointReached = () => {};
            
            waypoint._nextWaypoint = nextWaypoints[i].GetComponent<WaypointBehaviour>();
            waypoint = waypoint._nextWaypoint;
        }
        UpdateColor();
        waypoint._onWaypointReached = onFinalWaypointReached;
    }

private void OnTriggerEnter2D(Collider2D other)
    {
        if (_deactivated || !_canBeReached) return;
        
        var carController = other.gameObject.GetComponentInChildren<CarController>();
        if (!carController || !carController.IsDrivenByPlayer()) return;
        
        _onWaypointReached.Invoke();
        if (_nextWaypoint == null)
        {
            _spriteRenderer.color = Color.grey;   
        }
        else
        {
            gameObject.SetActive(false);
        }
        _indicatorManager.HideIndicator();
        _deactivated = true;

        if (_nextWaypoint != null)
        {
            _nextWaypoint._deactivated = false;
            _nextWaypoint._canBeReached = true;
            _nextWaypoint.gameObject.SetActive(true);
        }
        else
        {
            carController.KillPlayer();
        }
    }
}

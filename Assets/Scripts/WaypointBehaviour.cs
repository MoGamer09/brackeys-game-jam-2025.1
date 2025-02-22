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

    private void Awake()
    {
        _indicatorManager = GetComponentInChildren<IndicatorManager>();
        _deactivated = false;
        _indicatorManager.ShowIndicator();
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
        waypoint._onWaypointReached = onFinalWaypointReached;
    }

private void OnTriggerEnter2D(Collider2D other)
    {
        if (_deactivated || !_canBeReached) return;
        
        var carController = other.gameObject.GetComponentInChildren<CarController>();
        if (!carController || !carController.IsDrivenByPlayer()) return;
        
        _onWaypointReached.Invoke();
        GetComponent<SpriteRenderer>().color = Color.green;
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
            print("player killed");
            carController.KillPlayer();
        }
    }
}

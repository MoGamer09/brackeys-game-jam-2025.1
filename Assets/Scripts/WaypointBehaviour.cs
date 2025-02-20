using System;
using UnityEngine;

public class WaypointBehaviour : MonoBehaviour
{
    public Action OnWaypointReached;
    private bool _deactivated = false;
    private IndicatorManager _indicatorManager;

    private void Awake()
    {
        _indicatorManager = GetComponentInChildren<IndicatorManager>();
        _deactivated = false;
        _indicatorManager.ShowIndicator();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_deactivated) return;
        
        var carController = CarController.GetComponent<CarController>(other.gameObject);
        if (!carController || !carController.IsDrivenByPlayer()) return;
        
        OnWaypointReached.Invoke();
        GetComponent<SpriteRenderer>().color = Color.green;
        _indicatorManager.HideIndicator();
        _deactivated = true;
    }
}

using System;
using UnityEngine;

public class WaypointBehaviour : MonoBehaviour
{
    public Action OnWaypointReached;
    private bool _deactivated = false;

    private void Awake()
    {
        _deactivated = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_deactivated) return;
        
        var carController = CarController.GetComponent<CarController>(other.gameObject);
        if (!carController || !carController.IsDrivenByPlayer()) return;
        
        OnWaypointReached.Invoke();
        GetComponent<SpriteRenderer>().color = Color.green;
        _deactivated = true;
    }
}

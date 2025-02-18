using System;
using UnityEngine;

public class WaypointBehaviour : MonoBehaviour
{
    public Action onWaypointReached;
    private void OnCollisionEnter2D(Collision2D other)
    {
        var carController = other.gameObject.GetComponent<CarController>();
        if (carController && carController.IsDrivenByPlayer())
        {
            onWaypointReached.Invoke();
        }
    }
}

using System;
using UnityEngine;

public class ExplosionTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionEffect;

    [SerializeField]
    private float explosionScale = 2.0f;
    
    [SerializeField]
    private CarController _carController;

    private void Awake()
    {
        _carController = GetComponent<CarController>() ?? transform.parent.GetComponentInChildren<CarController>();
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        explosion.transform.localScale = Vector2.one * explosionScale;

        _carController.Explode();
    }
}

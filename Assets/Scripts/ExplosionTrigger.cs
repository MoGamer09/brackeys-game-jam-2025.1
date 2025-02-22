using System;
using UnityEngine;

public class ExplosionTrigger : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private GameObject fireTrailEffect;

    [SerializeField]
    private float explosionScale = 2.0f;
    
    [SerializeField]
    private CarController _carController;

    private void Awake()
    {
        _carController = GetComponent<CarController>() ?? transform.parent.GetComponentInChildren<CarController>();
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.GetComponent<ExplosionTrigger>())
            Instantiate(fireTrailEffect, other.transform.position, Quaternion.identity);
        else
            if (other.gameObject.GetComponent<ExplosionTrigger>()._carController.IsFollowingPath())
                Instantiate(fireTrailEffect, other.transform);
        
        var explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(explosion, 5.0f);
        explosion.transform.localScale = Vector2.one * explosionScale;

        _carController.Explode(other);
    }
    
    public CarController GetCarController() => _carController;
}

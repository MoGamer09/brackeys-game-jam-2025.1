using System;
using UnityEngine;

public class ExplosionTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionEffect;

    [SerializeField]
    private float explosionScale = 2.0f;

    [HideInInspector]
    public Action OnFail;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        explosion.transform.localScale = Vector2.one * explosionScale;
        
        OnFail?.Invoke();
    }
}

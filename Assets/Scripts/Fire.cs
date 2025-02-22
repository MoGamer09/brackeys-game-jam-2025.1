using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField]
    private float fireRate;

    [SerializeField]
    private int fireSpreadCount;

    [SerializeField] private float fireDistance;

    private float _fireAngle;
    private bool _fired;

    private void Start()
    {
        _fireAngle = 2 * Mathf.PI / fireSpreadCount;
        _fired = false;
    }
    private void FixedUpdate()
    {
        if (!_fired && Random.value < fireRate)
            RandomTick();
    }

    private void RandomTick()
    {
        var validAngles = new List<float>();
        var randomStart = Random.value * _fireAngle;
        for (var angle = randomStart; angle < 2 * Mathf.PI + randomStart; angle += _fireAngle)
        {
            if (!Physics2D.Raycast(transform.position, new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)), fireDistance))
                validAngles.Add(angle);
        }

        foreach (var angle in validAngles)
        {
            var newFire = Instantiate(this.gameObject,
                transform.position + fireDistance * (new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0.0f)),
                Quaternion.identity);
            newFire.GetComponent<Fire>().fireRate /= fireSpreadCount;
        }
        
        _fired = true;
    }
}

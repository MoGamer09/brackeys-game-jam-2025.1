using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CarPhysics : MonoBehaviour
{
    public float driftFactor = 0.8f;

    private Rigidbody2D _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        KillOrthogonalVelocity();
    }

    private void KillOrthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(_rb.linearVelocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(_rb.linearVelocity, transform.right);

        _rb.linearVelocity = forwardVelocity + rightVelocity * driftFactor;
    }

}

using UnityEngine;
using UnityEngine.Serialization;

public class Rotator : MonoBehaviour
{
    public float rotationTime;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LeanTween.rotateAroundLocal(gameObject, Vector3.forward, 360, rotationTime).setRepeat(-1).setLoopClamp();   
    }
}

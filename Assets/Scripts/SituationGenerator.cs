using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[Serializable]
public struct SituationData
{
    public GameObject car;
    public GameObject waypoint;
}

public class SituationGenerator : MonoBehaviour
{
    [SerializeField]
    private SituationData[] situations;

    private uint _situationIndex;

    private void Awake()
    {
        _situationIndex = 0;

        for (var i = 0; i < situations.Length; i++)
        {
            situations[i].car.SetActive(false);
            situations[i].waypoint.SetActive(false);
        }
    }

    public GameObject GenerateSituation(Action onFinished, Action onLevelComplete)
    {
        if (_situationIndex >= situations.Length)
        {
            onLevelComplete?.Invoke();
            return null;
        }
        var waypoint = situations[_situationIndex].waypoint;
        waypoint.SetActive(true);
        waypoint.GetComponent<WaypointBehaviour>().OnWaypointReached = onFinished;
        
        var car = situations[_situationIndex].car;
        car.SetActive(true);
        // var carController = car.GetComponent<CarController>();
        // var explosionTriggers = car.GetComponentsInChildren<ExplosionTrigger>();
        // foreach (var explosionTrigger in explosionTriggers)
        //     explosionTrigger.carController = carController;

        ++_situationIndex;
        return car;

    }
}

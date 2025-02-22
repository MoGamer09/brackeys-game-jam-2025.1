using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[Serializable]
public struct SituationData
{
    public GameObject car;
    public GameObject[] waypoints;
    public OrderData order;
}

//Only for the visual representation of the order
[Serializable]
public struct OrderData
{
    public int orderNumber;
    public string date;
    public string client;
    public string contactPerson;
    public string description;
    public float distance;
    public string departureTime;
    public string priorityLevel;
    public int payment;
}

public class SituationGenerator : MonoBehaviour
{
    [SerializeField]
    private SituationData[] situations;

    private int _situationIndex;

    private void Awake()
    {
        _situationIndex = 0;

        for (var i = 0; i < situations.Length; i++)
        {
            situations[i].car.SetActive(false);
            foreach (var waypoint in situations[i].waypoints)
                waypoint.SetActive(false);
        }
    }

    public (GameObject, OrderData?) GenerateSituation(Action onFinished, Action onLevelComplete)
    {
        if (_situationIndex >= situations.Length)
        {
            onLevelComplete?.Invoke();
            return (null, null);
        }
        var waypoints = situations[_situationIndex].waypoints;
        waypoints[0].SetActive(true);
        waypoints[0].GetComponent<WaypointBehaviour>().Init(onFinished, waypoints);
        
        var car = situations[_situationIndex].car;
        car.SetActive(true);
        car.GetComponentInChildren<CarController>().priority = _situationIndex;
        // var carController = car.GetComponent<CarController>();
        // var explosionTriggers = car.GetComponentsInChildren<ExplosionTrigger>();
        // foreach (var explosionTrigger in explosionTriggers)
        //     explosionTrigger.carController = carController;

        var order = situations[_situationIndex].order;
        
        ++_situationIndex;
        return (car, order);

    }
}

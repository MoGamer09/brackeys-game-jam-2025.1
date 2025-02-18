using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SituationGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject _waypoint;

    [SerializeField]
    private GameObject[] _carPrefabs;

    [SerializeField]
    private Vector2 _expanse;

    public GameObject GenerateSituation(Action onFinished)
    {
        // Random for testing
        print("test");
        var waypoint = Instantiate(_waypoint, RandomPosition(), Quaternion.identity);
        waypoint.GetComponent<WaypointBehaviour>().onWaypointReached = onFinished;
        var car = Instantiate(_carPrefabs[Random.Range(0, _carPrefabs.Length)], RandomPosition(), Quaternion.identity);
        return car;

    }

    private Vector2 RandomPosition()
    {
        return new Vector2(Random.Range(-_expanse.x, _expanse.x), Random.Range(-_expanse.y, _expanse.y));
    }
}

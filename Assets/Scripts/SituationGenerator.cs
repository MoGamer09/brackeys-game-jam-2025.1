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
        var randomPosition = RandomPosition();
        if (randomPosition.magnitude < _expanse.magnitude * 0.2f)
            randomPosition *= 3.0f;
        var waypoint = Instantiate(_waypoint, randomPosition, Quaternion.identity);
        waypoint.GetComponent<WaypointBehaviour>().OnWaypointReached = onFinished;
        var car = Instantiate(_carPrefabs[Random.Range(0, _carPrefabs.Length)], -randomPosition, Quaternion.identity);
        return car;

    }

    private Vector2 RandomPosition()
    {
        return new Vector2(Random.Range(-_expanse.x, _expanse.x), Random.Range(-_expanse.y, _expanse.y));
    }
}

using System.Collections.Generic;
using UnityEngine;

public struct RecordEntry
{
    public Vector2 position;
    public Quaternion rotation;
}

public class Recorder : MonoBehaviour
{
    private GameObject _activeCar;
    
    private bool _recording = false;
    private List<RecordEntry> _records;

    void Awake()
    {
        _records = new List<RecordEntry>();
    }
    
    private void FixedUpdate()
    {
        if (_recording)
            Record();
    }

    private void Record()
    {
        _records.Add(new RecordEntry {
            position = _activeCar.transform.position,
            rotation = _activeCar.transform.rotation
        });
    }

    public void StartRecording(GameObject car)
    {
        _activeCar = car;
        _recording = true;
        _records.Clear();
    }

    public List<RecordEntry> StopRecording()
    {
        _recording = false;
        return _records;
    }
}

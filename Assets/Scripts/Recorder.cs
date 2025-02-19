using System.Collections.Generic;
using UnityEngine;

public struct RecordEntry
{
    public Vector2 position;
    public Quaternion rotation;
    
    public RecordEntry[] adherents;
}

public class Recorder : MonoBehaviour
{
    private CarController _activeCar;
    
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
        _records.Add(_activeCar.PathEntry());
    }

    public void StartRecording(CarController car)
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

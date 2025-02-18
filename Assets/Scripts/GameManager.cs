using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(SituationGenerator), typeof(Recorder))]
public class GameManager : MonoBehaviour
{
    private SituationGenerator _situationGenerator;
    private Recorder _recorder;

    private uint _pathIndex;
    
    void Awake()
    {
        Application.targetFrameRate = 60;
        
        _situationGenerator = GetComponent<SituationGenerator>();
        _recorder = GetComponent<Recorder>();
    }

    void Start()
    {
        NextSituation();
    }

    void FixedUpdate()
    {
        ++_pathIndex;
    }

    void NextSituation()
    {
        var activeCar = ActiveCar();
        var nextCar = _situationGenerator.GenerateSituation(NextSituation);
        var oldPath = deepCopy(_recorder.StopRecording());
        _recorder.StartRecording(nextCar);
        _pathIndex = 0;
        
        if (!activeCar) return; // first car
        activeCar.SetPath(oldPath, PathIndex);
    }

    private uint PathIndex()
    {
        return _pathIndex;
    }

    [CanBeNull]
    public static CarController ActiveCar()
    {
        var carControllers = FindObjectsByType<CarController>(FindObjectsSortMode.None);
        foreach (var carController in carControllers)
            if (carController.IsDrivenByPlayer())
                return carController;
        return null;
    }

    private static RecordEntry[] deepCopy(List<RecordEntry> records)
    {
        var clonedRecords = new RecordEntry[records.Count];
        for (var i = 0; i < records.Count; i++)
            clonedRecords[i] = records[i];
        return clonedRecords;
    }
}

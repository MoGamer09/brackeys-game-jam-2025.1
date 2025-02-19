using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(SituationGenerator), typeof(Recorder))]
public class GameManager : MonoBehaviour
{
    private SituationGenerator _situationGenerator;
    private Recorder _recorder;

    private uint _pathIndex;
    
    private void Awake()
    {
        Application.targetFrameRate = 60;
        
        _situationGenerator = GetComponent<SituationGenerator>();
        _recorder = GetComponent<Recorder>();
    }

    private void Start()
    {
        NextSituation();
    }

    private void FixedUpdate()
    {
        ++_pathIndex;
    }

    private void NextSituation()
    {
        var activeCar = ActiveCar();
        var nextCar = _situationGenerator.GenerateSituation(NextSituation, NextLevel);
        var oldPath = deepCopy(_recorder.StopRecording());
        _recorder.StartRecording(nextCar.GetComponent<CarController>());
        _pathIndex = 0;
        
        if (!activeCar) return; // first car
        activeCar.SetPath(oldPath, PathIndex);
    }

    private void NextLevel()
    {
        print("next level");
        EditorApplication.isPlaying = true; // NOTE halting until level change
    }

    private uint PathIndex()
    {
        return _pathIndex;
    }

    [CanBeNull]
    public static CarController ActiveCar()
    {
        var carControllers = FindObjectsByType<CarController>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
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

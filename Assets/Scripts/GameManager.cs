using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SituationGenerator), typeof(Recorder))]
public class GameManager : MonoBehaviour
{
    private SituationGenerator _situationGenerator;
    private Recorder _recorder;

    private int _pathIndex;
    private bool _updatePathIndex;
    
    [SerializeField]
    private String nextScene;
    
    private UIManager _uiManager;
    
    private CarController[] _cars = Array.Empty<CarController>();
    
    private bool _shouldJumpToNextSituation;
    
    private void Awake()
    {
        Application.targetFrameRate = 60;
        
        _situationGenerator = GetComponent<SituationGenerator>();
        _recorder = GetComponent<Recorder>();
        _uiManager = FindFirstObjectByType<UIManager>();
        _cars = FindObjectsByType<CarController>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        _shouldJumpToNextSituation = false;
    }

    private void Start()
    {
        ActuallyLoadNextSituation();
    }

    private void FixedUpdate()
    {
        if (_updatePathIndex)
            ++_pathIndex;

        if (_shouldJumpToNextSituation && AllCarsDone())
        {
            ActuallyLoadNextSituation();
            _shouldJumpToNextSituation = false;
        }
    }

    private void ActuallyLoadNextSituation()
    {
        var activeCar = ActiveCar();
        var nextCar = _situationGenerator.GenerateSituation(QueryNextSituation, NextLevel);

        if (!nextCar) return;
        
        var oldPath = deepCopy(_recorder.StopRecording());
        _recorder.StartRecording(nextCar.GetComponentInChildren<CarController>());
        
        _pathIndex = 0;
        _updatePathIndex = true;

        foreach (var car in _cars)
        {
            car.RemoveTireMarks();
            car.ResetPath();
        }
        
        if (!activeCar) return; // first car
        activeCar.SetPath(oldPath, PathIndex);
    }

    private void NextLevel()
    {
        print("next level");
        // SceneManager.LoadScene(nextScene);
    }

    private void QueryNextSituation()
    {
        _shouldJumpToNextSituation = true;
    }

    private int PathIndex()
    {
        return _pathIndex;
    }

    private bool AllCarsDone()
    {
        return _cars.All(car => car.IsDone());
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

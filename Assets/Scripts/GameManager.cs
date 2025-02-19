using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SituationGenerator), typeof(Recorder))]
public class GameManager : MonoBehaviour
{
    private SituationGenerator _situationGenerator;
    private Recorder _recorder;

    private uint _pathIndex;
    private bool _updatePathIndex;
    
    [SerializeField]
    private String nextScene;
    
    private UIManager _uiManager;
    
    private void Awake()
    {
        Application.targetFrameRate = 60;
        
        _situationGenerator = GetComponent<SituationGenerator>();
        _recorder = GetComponent<Recorder>();
        _uiManager = FindFirstObjectByType<UIManager>();
    }

    private void Start()
    {
        NextSituation();
    }

    private void FixedUpdate()
    {
        if (_updatePathIndex)
            ++_pathIndex;
    }

    private void NextSituation()
    {
        var activeCar = ActiveCar();
        var nextCar = _situationGenerator.GenerateSituation(NextSituation, NextLevel, GameOver);
        var oldPath = deepCopy(_recorder.StopRecording());
        _recorder.StartRecording(CarController.GetComponent<CarController>(nextCar));
        
        _pathIndex = 0;
        _updatePathIndex = true;
        
        if (!activeCar) return; // first car
        activeCar.SetPath(oldPath, PathIndex);
    }

    private void GameOver()
    {
        _uiManager.GameOver();
        _updatePathIndex = false;
        ActiveCar()?.KillPlayer();
    }

    private void NextLevel()
    {
        print("next level");
        SceneManager.LoadScene(nextScene);
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

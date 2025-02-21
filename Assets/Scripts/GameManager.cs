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
    private bool _reverse;

    [SerializeField] private String nextScene;

    private UIManager _uiManager;

    private CarController[] _cars = Array.Empty<CarController>();

    private bool _shouldJumpToNextSituation;

    private OrderVisualizer _orderVisualizer;
    private InputHandler _inputHandler;
    
    private List<GameObject> _pathRenderers = new List<GameObject>();
    public GameObject pathRendererPrefab;


    private void Awake()
    {
        Application.targetFrameRate = 60;

        _situationGenerator = GetComponent<SituationGenerator>();
        _recorder = GetComponent<Recorder>();
        _uiManager = FindFirstObjectByType<UIManager>();
        _cars = FindObjectsByType<CarController>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        _shouldJumpToNextSituation = false;
        _orderVisualizer = FindFirstObjectByType<OrderVisualizer>();
        _inputHandler = GetComponent<InputHandler>();
    }

    private void Start()
    {
        ActuallyLoadNextSituation();
    }

    private void FixedUpdate()
    {
        if (_updatePathIndex)
        {
            if (!_reverse)
            {
                ++_pathIndex;
            }
            else if (_pathIndex > 0)
            {
                --_pathIndex;
            }
        }

        if (_shouldJumpToNextSituation && AllCarsDone())
        {
            ActuallyLoadNextSituation();
            _shouldJumpToNextSituation = false;
        }
    }

    private void ActuallyLoadNextSituation()
    {
        var activeCar = ActiveCar();
        if (activeCar != null)
        {
            activeCar.GetComponentInChildren<CarController>().playerControlled = false; //Stop player driving
        }

        var situation = _situationGenerator.GenerateSituation(QueryNextSituation, NextLevel);
        var nextCar = situation.Item1;

        if (!nextCar) return; //Was last car

        var oldPath = deepCopy(_recorder.StopRecording()); //Stop recording

        if (activeCar) //not first car
            activeCar.SetPath(oldPath, PathIndex);

        /*
         nextCar.SetActive(false);
        _reverse = true; //reverse time
        _updatePathIndex = true; // Stop time
        */

        _updatePathIndex = false;

        _orderVisualizer.ShowOrder(situation.Item2 ?? new OrderData(), () =>
        {
            //Finish grace period
            foreach (var car in _cars)
            {
                car.RemoveTireMarks();
                car.ResetPath();
            }

            _pathIndex = 0; //Reset Time
            _reverse = false;
            nextCar.SetActive(true);
            nextCar.GetComponentInChildren<CarController>().playerControlled = true; //Start player driving

            var startGame = new Action(() =>
            {
                HideCarPaths();
                _recorder.StartRecording(nextCar.GetComponentInChildren<CarController>());
                _updatePathIndex = true; //Start time again
                _inputHandler.OnInputMade = () => { };
            });

            //Wait for player input to start

            ShowCarPaths();
            
            _inputHandler.OnInputMade = startGame;
            
            //Time between accepted contract and start
        });


        //This is the grace period.
    }

    private void HideCarPaths()
    {
        foreach (var pathRenderer in _pathRenderers)
        {
            Destroy(pathRenderer);
        }
        
        _pathRenderers.Clear();
    }

    private void ShowCarPaths()
    {
        foreach (var carController in _cars)
        {
            var newPath = Instantiate(pathRendererPrefab);
            var lr = newPath.GetComponent<LineRenderer>();
            var path = carController.GetPath();
            lr.positionCount = carController.GetPathSize();
            lr.SetPositions(path.Select(p => (Vector3)p.position).ToList<Vector3>().GetRange(0, lr.positionCount).ToArray());
            _pathRenderers.Add(newPath);
        }
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
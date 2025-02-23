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

    [SerializeField] private String nextScene;

    private UIManager _uiManager;

    private CarController[] _cars = Array.Empty<CarController>();

    private bool _shouldJumpToNextSituation;

    private OrderVisualizer _orderVisualizer;
    private InputHandler _inputHandler;

    private List<GameObject> _pathRenderers = new List<GameObject>();
    public GameObject pathRendererPrefab;

    private EndScreenManager _endScreenManager;
    private ScreenTinter _screenTinter;
    private MusicManager _musicManager;

    private int _pathIndexDelta = 1;

    [SerializeField] private GameObject[] speedUpImages;
    [SerializeField] private GameObject speedButton;

    public GameObject retryButton;


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
        _endScreenManager = GetComponent<EndScreenManager>();
        _screenTinter = GetComponentInChildren<ScreenTinter>();
        _musicManager = FindFirstObjectByType<MusicManager>();
    }

    private void Start()
    {
        ActuallyLoadNextSituation();
        SetSpeed(1);
        retryButton.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (_updatePathIndex)
        {
            _pathIndex += _pathIndexDelta;
        }

        if (_shouldJumpToNextSituation && AllCarsDone())
        {
            ActuallyLoadNextSituation();
            _shouldJumpToNextSituation = false;
        }
    }

    private void ActuallyLoadNextSituation()
    {
        speedButton.SetActive(false);
        retryButton.SetActive(false);
        
        SetSpeed(1); //Set Speed to normal

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

        _screenTinter.SetScreenTint(true);

        _orderVisualizer.ShowOrder(situation.Item2 ?? new OrderData(), () =>
        {
            //Finish grace period
            foreach (var car in _cars)
            {
                car.RemoveTireMarks();
                car.ResetPath();
            }

            _pathIndex = 0; //Reset Time
            _pathIndexDelta = 1; //Reset Speed
            nextCar.SetActive(true);
            nextCar.GetComponentInChildren<CarController>().playerControlled = true; //Start player driving

            var startGame = new Action(() =>
            {
                HideCarPaths();
                _recorder.StartRecording(nextCar.GetComponentInChildren<CarController>());
                _updatePathIndex = true; //Start time again
                _inputHandler.OnInputMade = () => { };
                retryButton.SetActive(true);
                _musicManager?.AddMusicLayer();
            });

            //Wait for player input to start
            _screenTinter.SetScreenTint(false);

            ShowCarPaths();

            _inputHandler.OnInputMade = startGame;

            //Time between accepted contract and start
            var fireTrails = GameObject.FindGameObjectsWithTag("Firetrail");
            foreach (var fireTrail in fireTrails)
            {
                Destroy(fireTrail);   
            }
        });


        //This is the grace period.
    }

    private void SetSpeed(int speed)
    {
        switch (speed)
        {
            case 1:
                for (var i = 0; i < speedUpImages.Length; i++)
                {
                    speedUpImages[i].SetActive(i == 0);   
                }

                _pathIndexDelta = 1;
                break;
            case 2:
                for (var i = 0; i < speedUpImages.Length; i++)
                {
                    speedUpImages[i].SetActive(i == 1);   
                }

                _pathIndexDelta = 2;
                break;
            case 6:
                for (var i = 0; i < speedUpImages.Length; i++)
                {
                    speedUpImages[i].SetActive(i == 2);   
                }

                _pathIndexDelta = 6;
                break;
            default:
                SetSpeed(1);
                break;
        }
    }

    public void ChangeSpeed()
    {
        switch (_pathIndexDelta)
        {
            case 1:
                SetSpeed(2);
                break;
            case 2:
                SetSpeed(6);
                break;
            case 6:
                SetSpeed(1);
                break;
            default:
                SetSpeed(1);
                break;
        }
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
            lr.SetPositions(path.Select(p => (Vector3) p.position).ToList<Vector3>().GetRange(0, lr.positionCount)
                .ToArray());
            _pathRenderers.Add(newPath);
        }
    }

    private void NextLevel()
    {
        _endScreenManager.ShowEndScreen();
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void QueryNextSituation()
    {
        speedButton.SetActive(true);
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
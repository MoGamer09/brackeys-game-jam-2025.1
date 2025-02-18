using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(SituationGenerator), typeof(Recorder))]
public class GameManager : MonoBehaviour
{
    private SituationGenerator _situationGenerator;
    private Recorder _recorder;
    
    void Awake()
    {
        _situationGenerator = GetComponent<SituationGenerator>();
        _recorder = GetComponent<Recorder>();
    }

    void Start()
    {
        NextSituation();
    }

    void NextSituation()
    {
        var nextCar = _situationGenerator.GenerateSituation(NextSituation);
        var oldPath = _recorder.StopRecording();
        var activeCar = ActiveCar();
        if (!activeCar) throw new UnityException("No active car");
        activeCar.SetPath(oldPath.ToArray());
        _recorder.StartRecording(nextCar);
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
}

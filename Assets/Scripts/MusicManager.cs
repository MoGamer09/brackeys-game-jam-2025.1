using System;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource[] loopSources;
    private int _currentLayer = 0;
    private DateTime startTime;

    private void Awake()
    {
        startTime = DateTime.Now;
    }

    public void AddMusicLayer()
    {
        if (_currentLayer >= loopSources.Length) return;
        
        loopSources[_currentLayer].mute = false;
        var clip = loopSources[_currentLayer].clip;
        var clipLength = (double)clip.samples / clip.frequency;
        var elapsedTime = (DateTime.Now - startTime).TotalSeconds;
        
        loopSources[_currentLayer].PlayDelayed( (float) (clipLength - elapsedTime % clipLength));
        
        _currentLayer++;
    }
}

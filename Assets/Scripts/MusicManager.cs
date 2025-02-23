using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource[] loopSources;
    private int _currentLayer = 0;

    public void AddMusicLayer()
    {
        if (_currentLayer >= loopSources.Length) return;
        
        loopSources[_currentLayer].mute = false;
        var clip = loopSources[_currentLayer].clip;
        var clipLength = (double)clip.samples / clip.frequency;
        
        loopSources[_currentLayer].PlayScheduled( AudioSettings.dspTime +  (clipLength - AudioSettings.dspTime % clipLength));
        
        _currentLayer++;
    }
}

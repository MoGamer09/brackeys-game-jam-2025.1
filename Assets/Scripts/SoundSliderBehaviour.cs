using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSliderBehaviour : MonoBehaviour
{
    [SerializeField]
    private AudioMixer mixer;
    
    private Slider _slider;

    public string varName;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _slider.onValueChanged.AddListener(OnValueChange);
    }

    private void OnValueChange(float value)
    {
        mixer.SetFloat(varName, Mathf.Log10(value) * 20);
    }
}

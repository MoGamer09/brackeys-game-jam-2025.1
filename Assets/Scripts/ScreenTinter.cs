using UnityEngine;

public class ScreenTinter : MonoBehaviour
{
    public GameObject tinter;
    public Color tinterColor;
    
    public void SetScreenTint(bool value)
    {
        LeanTween.color((RectTransform)tinter.transform, value ? tinterColor : Color.clear, 0.3f).setEase(LeanTweenType.easeInOutCirc);
    }
}

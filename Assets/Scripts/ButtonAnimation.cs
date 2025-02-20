using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        _text.transform.localScale = Vector3.one;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        LeanTween.scale(_text.rectTransform, Vector3.one * 1.2f, 0.2f).setEaseInOutExpo();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.scale(_text.rectTransform, Vector3.one, 0.2f).setEaseInOutExpo();
    }
}

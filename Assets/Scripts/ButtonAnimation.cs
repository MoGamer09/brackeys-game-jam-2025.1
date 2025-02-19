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

    public void OnPointerEnter(PointerEventData eventData)
    {
        _text.transform.localScale *= 1.2f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _text.transform.localScale /= 1.2f;
    }
}

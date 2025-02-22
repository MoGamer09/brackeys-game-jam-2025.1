using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(OrderTextPopulator))]
public class OrderVisualizer : MonoBehaviour
{
    private Action _onAccept;
    public Vector2 showPosition;
    public Vector2 hidePosition;
    public GameObject stamp;
    public Button AcceptButton;
    
    private OrderTextPopulator _orderTextPopulator;

    private void Start()
    {
        transform.position = hidePosition;
        _orderTextPopulator = GetComponent<OrderTextPopulator>();
    }

    public void ShowOrder(OrderData order, Action onAccept)
    {
        //Hide accapt
        LeanTween.scale(stamp, Vector3.one * 3, 0f);
        stamp.GetComponent<SpriteRenderer>().color = Color.clear;
        
        //Show panel
        LeanTween.moveLocal(gameObject, showPosition, 0.5f).setEase(LeanTweenType.easeInOutCubic);
        AcceptButton.gameObject.SetActive(true);
        
        _orderTextPopulator.UpdateOrderText(order);
        
        _onAccept = onAccept;
    }

    public void AcceptOrder()
    {
        AcceptButton.gameObject.SetActive(false);
        var seq = LeanTween.sequence();
        seq.append(() =>
        {
            LeanTween.scale(stamp, Vector3.one, 0.4f).setEase(LeanTweenType.easeOutExpo);
            LeanTween.color(stamp, Color.white, 0.1f);
        });
        seq.append(1);
        seq.append(LeanTween.moveLocal(gameObject, hidePosition, 0.5f).setEase(LeanTweenType.easeInOutCubic));
        seq.append((() => { _onAccept?.Invoke(); }));
    }
}
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderVisualizer : MonoBehaviour
{
    private Action _onAccept;
    public Vector2 showPosition;
    public Vector2 hidePosition;
    public GameObject stamp;
    public Button AcceptButton;
        
    public TMP_Text orderNumberTxt,
        dateTxt,
        contactTxt,
        clientTxt,
        descriptionTxt,
        distanceTxt,
        departureTimeTxt,
        priorityLevelTxt,
        paymentTxt;

    private void Start()
    {
        transform.position = hidePosition;
    }

    public void ShowOrder(OrderData order, Action onAccept)
    {
        //Hide accapt
        LeanTween.scale(stamp, Vector3.one * 3, 0f);
        LeanTween.color(stamp, Color.clear, 0f);
        
        //Show panel
        LeanTween.moveLocal(gameObject, showPosition, 0.5f).setEase(LeanTweenType.easeInOutCubic);
        AcceptButton.gameObject.SetActive(true);
        
        orderNumberTxt.text = "Order No. " + order.orderNumber;
        dateTxt.text = order.date;
        contactTxt.text = "Contact Person: " + order.contactPerson;
        clientTxt.text = "Client: " + order.client;
        descriptionTxt.text = "Description: \n" + order.description;
        distanceTxt.text = "Distance: " + order.distance + " km";
        departureTimeTxt.text = "Departure Time: " + order.departureTime;
        priorityLevelTxt.text = "Priority Level: " + order.priorityLevel;
        paymentTxt.text = "Payment Upon Completion: " + order.payment;
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
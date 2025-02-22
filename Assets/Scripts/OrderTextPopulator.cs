using TMPro;
using UnityEngine;

public class OrderTextPopulator : MonoBehaviour
{
    public TMP_Text orderNumberTxt,
        dateTxt,
        contactTxt,
        clientTxt,
        descriptionTxt,
        distanceTxt,
        departureTimeTxt,
        priorityLevelTxt,
        paymentTxt;

    public void UpdateOrderText(OrderData order)
    {
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
}

using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    [SerializeField] private GameObject container;
    [SerializeField] private TMPro.TextMeshProUGUI titleText, descriptionText, deliveryTimeText;

    private void Notify(Notification notification) // Null checks later
    {
        titleText.text = notification.title;
        descriptionText.text = notification.description;
        deliveryTimeText.text = notification.deliveryTime;

        StartCoroutine(ShowNotification());

        System.Collections.IEnumerator ShowNotification()
        {
            container.SetActive(true);
            yield return new WaitForSeconds(3f);
            container.SetActive(false);
        }
    }

    void OnEnable() { Notification.Created += Notify; }
    void OnDisable() { Notification.Created -= Notify; }
}

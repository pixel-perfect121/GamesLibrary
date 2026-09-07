using UnityEngine;

public sealed class NotificationManager : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI titleText, descriptionText, deliveryTimeText;

    private Animator animator;
    private readonly int Animate = Animator.StringToHash("Animate");
    private readonly System.Collections.Generic.Queue<Notification> queue = new();
    private bool isBusy;

    void Awake() { animator = GetComponent<Animator>(); }

    private void Notify(Notification notification)
    {
        if (animator == null || titleText == null || descriptionText == null || deliveryTimeText == null) return;
        if (isBusy) { queue.Enqueue(notification); return; }

        isBusy = true;

        notification.SetDeliveryTime(System.DateTime.UtcNow);

        titleText.text = notification.title;
        descriptionText.text = notification.description;
        deliveryTimeText.text = $"Delivery time: {notification.DeliveryTime:hh:mm(tt)}";

        animator.ResetTrigger(Animate); animator.SetTrigger(Animate);
    }

    public void NotifyNext()
    {
        isBusy = false;

        if (queue.Count > 0) Notify(queue.Dequeue());
    }

    void OnEnable() { Notification.Created += Notify; }
    void OnDisable() { Notification.Created -= Notify; }
}

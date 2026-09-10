using UnityEngine;

public sealed class NotificationManager : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI titleText, descriptionText, deliveryTimeText;

    private Animator animator;
    private readonly int AnimateHash = Animator.StringToHash("Animate");
    private readonly System.Collections.Generic.Queue<Notification> notificationQueue = new();
    private bool isBusy;

    void Awake() { animator = GetComponent<Animator>(); }

    private void Notify(Notification notification)
    {
        if (animator == null || titleText == null || descriptionText == null || deliveryTimeText == null) return;
        if (isBusy) { notificationQueue.Enqueue(notification); return; }

        isBusy = true;

        notification.SetDeliveryTime(System.DateTime.UtcNow);

        titleText.text = notification.title;
        descriptionText.text = notification.description;
        deliveryTimeText.text = notification.DeliveryTime.ToString("hh:mm(tt)");

        animator.ResetTrigger(AnimateHash);
        animator.SetTrigger(AnimateHash);
    }

    public void Finished()
    {
        isBusy = false;

        if (notificationQueue.TryDequeue(out Notification notification)) Notify(notification);
    }

    void OnEnable() { Notification.Created += Notify; }
    void OnDisable() { Notification.Created -= Notify; }
}

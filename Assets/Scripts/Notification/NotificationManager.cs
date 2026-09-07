using UnityEngine;

public sealed class NotificationManager : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI titleText, descriptionText;

    private Animator animator;
    private readonly int Animate = Animator.StringToHash("Animate");
    private readonly System.Collections.Generic.Queue<Notification> notificationQueue = new();
    private bool isBusy;

    void Awake() { animator = GetComponent<Animator>(); }

    private void OnNotificationCreated(Notification notification)
    {
        if (animator == null || titleText == null || descriptionText == null) return;
        if (isBusy) { notificationQueue.Enqueue(notification); return; }

        isBusy = true;

        titleText.text = notification.title;
        descriptionText.text = notification.description;

        animator.ResetTrigger(Animate); animator.SetTrigger(Animate);
    }

    public void NotifyNext()
    {
        isBusy = false;

        if (notificationQueue.Count > 0) OnNotificationCreated(notificationQueue.Dequeue());
    }

    void OnEnable() { Notification.Created += OnNotificationCreated; }
    void OnDisable() { Notification.Created -= OnNotificationCreated; }
}

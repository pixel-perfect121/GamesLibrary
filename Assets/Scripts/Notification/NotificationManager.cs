using UnityEngine;

public sealed class NotificationManager : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI titleText, descriptionText;

    private Animator animator;
    private bool isBusy;
    private readonly int Animate = Animator.StringToHash("Animate");
    private readonly System.Collections.Generic.Queue<Notification> queue = new();

    void Awake() { animator = GetComponent<Animator>(); }

    private void Notify(Notification notification)
    {
        if (titleText == null || descriptionText == null) return;
        if (isBusy) { queue.Enqueue(notification); return; }

        isBusy = true;

        titleText.text = notification.title;
        descriptionText.text = notification.description;

        if (animator != null)
        {
            animator.ResetTrigger(Animate);
            animator.SetTrigger(Animate);
        }
    }

    public void NotifyNext()
    {
        isBusy = false;

        if (queue.Count > 0) Notify(queue.Dequeue());
    }

    void OnEnable() { Notification.Created += Notify; }
    void OnDisable() { Notification.Created -= Notify; }
}

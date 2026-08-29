using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI titleText, descriptionText, deliveryTimeText;

    private Animator animator;
    private readonly int AnimateHash = Animator.StringToHash("Animate");

    private readonly System.Collections.Generic.Queue<Notification> queue = new();
    private bool isBusy;

    void Awake() { animator = GetComponent<Animator>(); }

    private void Notify(Notification notification)
    {
        if (isBusy) { queue.Enqueue(notification); return; }

        isBusy = true;

        if (titleText != null) titleText.text = notification.title;
        if (descriptionText != null) descriptionText.text = notification.description;
        if (deliveryTimeText != null) deliveryTimeText.text = notification.deliveryTime;

        if (animator != null)
        {
            animator.ResetTrigger(AnimateHash);
            animator.SetTrigger(AnimateHash);
        }
    }

    public void Finished()
    {
        isBusy = false;

        if (queue.Count != 0) Notify(queue.Dequeue());
    }

    void OnEnable() { Notification.Created += Notify; }
    void OnDisable() { Notification.Created -= Notify; }
}

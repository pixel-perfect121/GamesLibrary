using System;

public struct Notification
{
    public readonly string title, description;
    public readonly DateTime creationTime;
    public DateTime DeliveryTime { get; private set; }

    public static event Action<Notification> Created;

    public Notification(string title, string description)
    {
        this.title = title; this.description = description;
        creationTime = DateTime.UtcNow; DeliveryTime = new();

        Created?.Invoke(this);
    }

    public void SetDeliveryTime(DateTime deliveryTime) => DeliveryTime = deliveryTime;
}

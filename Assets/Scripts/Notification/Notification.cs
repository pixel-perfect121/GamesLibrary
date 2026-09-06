using System;

/// <summary>Notifications across the game, useful for sending FIX BEDROCK MOJANG 🗿🗿🗿.</summary>
public struct Notification
{
    /// <summary>Subject's title or name of sender.</summary>
    public readonly string title;
    /// <summary>Content that will be sent, *cough* FIX BEDROCK MOJANG *cough*.</summary>
    public readonly string description;
    /// <summary>Time at which this object was created.</summary>
    public readonly DateTime creationTime;
    /// <summary>Time at which this object was delivered.</summary>
    public DateTime DeliveryTime { get; private set; }

    /// <summary>Automatically invoked when creating new notification object.</summary>
    public static event Action<Notification> Created;

    /// <summary>Create and invokes Created event.</summary>
    /// <param name="title">Name of sender or subject.</param>
    /// <param name="message">Content that will be displayed under title.</param>
    public Notification(string title, string message)
    {
        this.title = title; this.description = message;
        creationTime = DateTime.UtcNow; DeliveryTime = new();

        Created?.Invoke(this);
    }

    public void SetDeliveryTime(DateTime deliveryTime) => DeliveryTime = deliveryTime;
}

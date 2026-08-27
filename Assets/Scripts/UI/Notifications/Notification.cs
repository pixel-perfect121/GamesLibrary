using System;

/// <summary>Popup UI communication system.</summary>
public readonly struct Notification
{
    /// <summary>Name of sender or title of subject.</summary>
    public readonly string title;
    /// <summary>Extra information that will be sent.</summary>
    public readonly string description;
    /// <summary>Time this notification will be delivered in, formatted as hh:mm(AM/PM).</summary>
    public readonly string deliveryTime;

    /// <summary>Delegate storing the created notification, invoked when creating one.</summary>
    public static event Action<Notification> Created;

    /// <summary>Create and broadcast a notification object.</summary>
    /// <param name="title">Name of sender or title of subject.</param>
    /// <param name="description">Extra information that will be sent.</param>
    public Notification(string title, string description)
    {
        this.title = title; this.description = description;
        deliveryTime = DateTime.Now.ToString("hh:mm(tt)");

        Created?.Invoke(this);
    }
}

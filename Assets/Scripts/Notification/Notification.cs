using System;

/// <summary>Game-wide notifying system.</summary>
public readonly struct Notification
{
    /// <summary>Title of subject or name of sender.</summary>
    public readonly string title;
    /// <summary>Extra information that is carried with the object.</summary>
    public readonly string description;
    /// <summary>The date at which this object was created at.</summary>
    public readonly DateTime creationTime;

    /// <summary>Event carrying information about the object.</summary>
    public static event Action<Notification> Created;

    /// <summary>Create and invoke Created event.</summary>
    /// <param name="title">Title of subject or name of sender.</param>
    /// <param name="description">Extra information.</param>
    public Notification(string title, string description)
    {
        this.title = title; this.description = description;
        creationTime = DateTime.UtcNow;

        Created?.Invoke(this);
    }
}

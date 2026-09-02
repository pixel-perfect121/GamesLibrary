using System;

/// <summary>Type of modification done.</summary>
public enum Modification { Description, Rating, Both }

/// <summary>Struct holding information about a given game.</summary>
[Serializable]
public struct GameInfo
{
    /// <summary>Name or title of the game.</summary>
    public readonly string Title { get; }
    /// <summary>Extra information about the game.</summary>
    public string Description { get; private set; }
    /// <summary>How much you liked the game.</summary>
    /// <remarks>Idealy, this should goes from 0 to 5, but don't let this limit you 🗿.</remarks>
    public int Rating { get; private set; }

    /// <summary>Fires when creating an instance of this object.</summary>
    public static event Action<GameInfo> Created;
    /// <summary>Fires when using any modify method.</summary>
    public static event Action<GameInfo, Modification> Modified;

    /// <summary>Create and invoke Created event.</summary>
    /// <param name="name">Name or title of the game.</param>
    /// <param name="description">Extra information about the game.</param>
    /// <param name="rating">How much you liked the game.</param>
    public GameInfo(string name, string description, int rating)
    {
        Title = name; Description = description; Rating = rating;
        Created?.Invoke(this);
    }

    /// <summary>Change description and invoke Modified event.</summary>
    public void Modify(string newDescription)
    {
        Description = newDescription; // Maybe store old data and send it via the event
        Modified?.Invoke(this, Modification.Description);
    }
    /// <summary>Change rating and invoke Modified event.</summary>
    public void Modify(int newRating)
    {
        Rating = newRating;
        Modified?.Invoke(this, Modification.Rating);
    }
    /// <summary>Change description, rating, and invoke Modified event.</summary>
    public void Modify(string newDescription, int newRating)
    {
        Description = newDescription; Rating = newRating;
        Modified?.Invoke(this, Modification.Both);
    }

    /// <summary>Gives a string associated with the object.</summary>
    /// <returns>returns an ID for the game, formatted as lower_case_game</returns>
    public readonly string GetID() => $"{Title.ToLowerInvariant().Replace(" ", "_")}_game";
}

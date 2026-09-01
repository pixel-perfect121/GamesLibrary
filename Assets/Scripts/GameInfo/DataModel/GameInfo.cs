/// <summary>Struct holding information about a given game.</summary>
[System.Serializable]
public struct GameInfo
{
    /// <summary>Name or title of the game.</summary>
    public readonly string title;
    /// <summary>Extra information about the game.</summary>
    public string description;
    /// <summary>How much you liked the game. <br></br>
    /// Idealy, this should goes from 0 to 5, but don't let this limit you 🗿.</summary>
    public int rating;

    /// <summary>Event that fired when creating an instance of this object.</summary>
    public static event System.Action<GameInfo> Created;

    /// <summary>Create and invoke Created event.</summary>
    /// <param name="name">Name or title of the game.</param>
    /// <param name="description">Extra information about the game.</param>
    /// <param name="rating">How much you liked the game.</param>
    public GameInfo(string name, string description, int rating)
    {
        this.title = name; this.description = description; this.rating = rating;
        // Modification of description and rating will come later
        Created?.Invoke(this);
    }

    /// <summary>Gives the ID of the GameInfo object.</summary>
    /// <returns>returns an ID for the game formatted as lower_case_game</returns>
    public readonly string GetID() => $"{title.ToLowerInvariant().Replace(" ", "_")}_game";
}

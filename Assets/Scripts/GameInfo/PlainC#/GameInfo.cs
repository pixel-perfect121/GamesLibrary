[System.Serializable]
public struct GameInfo
{
    public readonly string name;
    public string description;
    public int rating;

    public static event System.Action<GameInfo> Created, Modified;

    public GameInfo(string name, string description, int rating)
    {
        this.name = name; this.description = description;
        this.rating = rating;

        Created?.Invoke(this);
    }

    public readonly string GetID() => $"{name.ToLower().Replace(" ", "_")}_game";

    public void ModifyDescription(string newDescription)
    {
        description = newDescription;

        Modified?.Invoke(this);
    }
    public void ModifyRating(int newRating)
    {
        rating = newRating;

        Modified?.Invoke(this);
    }
    public void ModifyDescriptionAndRating(string newDescription, int newRating)
    {
        ModifyDescription(newDescription);
        ModifyRating(newRating);

        Modified?.Invoke(this);
    }
}

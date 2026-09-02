using UnityEngine;

public static class GameInfoManager
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        GameInfo.Created += OnGameInfoCreated;
        GameInfo.Modified += OnGameInfoModified;
    }

    private static void OnGameInfoCreated(GameInfo gameInfo)
    {
        new Notification("Game added", $"{gameInfo.Title} has been added");
    }
    private static void OnGameInfoModified(GameInfo gameInfo, Modification modification)
    {
        string message = modification switch
        {
            Modification.Description => "\'s description has been modified",
            Modification.Rating => "\'s rating has been modified",
            Modification.Both => "has received a treatment for baldness",
            _ => "has remained the same. Somethings are just better this way"
        };

        new Notification("Game modified", $"{gameInfo.Title} {message}");
    }
}

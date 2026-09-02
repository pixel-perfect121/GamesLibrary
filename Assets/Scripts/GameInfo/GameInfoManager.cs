using UnityEngine;
using _SaveManager;

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
        if (!SaveManager.GameData.GameInfoDictionary.TryAdd(gameInfo.GetID(), gameInfo))
        {
            new Notification("Game already exists", $"{gameInfo.Title} already exists in the database");
            return;
        }

        SaveManager.RequestSave(Method.Async);

        new Notification("Game added", $"{gameInfo.Title} has been added");
    }
    private static void OnGameInfoModified(GameInfo gameInfo, Modification modification)
    {
        if (!SaveManager.GameData.GameInfoDictionary.ContainsKey(gameInfo.GetID()))
        {
            new Notification("Game does not exist", "The given ID doesn't correspond to any game");
            return;
        }

        SaveManager.RequestSave(Method.Async);

        new Notification("Game modified", $"{gameInfo.Title} has been modified");
    }
}

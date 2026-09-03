using UnityEngine;
using System.Collections.Generic;
using _SaveManager;
using System.Linq;

public static class GameInfoManager
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        GameInfo.Created += OnGameInfoCreated;
        GameInfo.Modified += OnGameInfoModified;
    }

    public static IReadOnlyList<GameInfo> RequestGameInfos() => SaveManager.GameData.GameInfoDictionary.Values.ToList();

    private static void OnGameInfoCreated(GameInfo gameInfo)
    {
        if (SaveManager.GameData.GameInfoDictionary.ContainsKey(gameInfo.GetID()))
        {
            new Notification("Game already exists", $"{gameInfo.Title} already exists in the database");
            return;
        }

        SaveManager.GameData.GameInfoDictionary.Add(gameInfo.GetID(), gameInfo);
        SaveManager.RequestSave(Method.Async);

        new Notification("Game added", $"{gameInfo.Title} has been added");
    }
    private static void OnGameInfoModified(GameInfo gameInfo, Modification modification)
    {
        if (!SaveManager.GameData.GameInfoDictionary.ContainsKey(gameInfo.GetID()))
        {
            new Notification("Game does not exist", "The given ID does not correspond to any game");
            return;
        }

        SaveManager.RequestSave(Method.Async);

        new Notification("Game modified", $"{gameInfo.Title} has been modified");
    }
}

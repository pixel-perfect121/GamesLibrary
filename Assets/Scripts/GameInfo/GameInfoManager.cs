using UnityEngine;
using System.Text.Json;
using System.Collections.Generic;

public class GameInfoManager : MonoBehaviour
{
    private readonly Dictionary<string, GameInfo> gameInfos = new();

    private void OnGameInfoCreated(GameInfo gameInfo)
    {
        Debug.Log($"Name: {gameInfo.name}, Description: {gameInfo.description}, Rating: {gameInfo.rating}");
        gameInfos.Add(gameInfo.ID, gameInfo);

        string json = JsonSerializer.Serialize(gameInfos, new JsonSerializerOptions() { IncludeFields = true, WriteIndented = true });
        System.IO.File.WriteAllText(System.IO.Path.Combine(Application.persistentDataPath, "GameInfo.json"), json);
    }
    private void OnGameInfoModified(GameInfo gameInfo)
    {
        Debug.Log($"{gameInfo.name} has been modified");
    }

    void OnEnable()
    {
        GameInfo.Created += OnGameInfoCreated;
        GameInfo.Modified += OnGameInfoModified;
    }
    void OnDisable()
    {
        GameInfo.Created -= OnGameInfoCreated;
        GameInfo.Modified -= OnGameInfoModified;
    }
}

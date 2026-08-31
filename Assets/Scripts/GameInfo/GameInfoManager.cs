using UnityEngine;
using System.Collections.Generic;

public class GameInfoManager : MonoBehaviour
{
    private readonly Dictionary<string, GameInfo> gamesDictionary = new();

    private void OnGameInfoCreated(GameInfo gameInfo)
    {
        gamesDictionary.Add(gameInfo.GetID(), gameInfo);
        Debug.Log($"Name: {gameInfo.name}, Description: {gameInfo.description}, Rating: {gameInfo.rating}, \nID: {gameInfo.GetID()}");
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

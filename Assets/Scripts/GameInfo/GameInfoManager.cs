using UnityEngine;

public static class GameInfoManager
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        GameInfo.Created += OnGameInfoCreated;
    }

    private static void OnGameInfoCreated(GameInfo gameInfo)
    {
        
    }
}

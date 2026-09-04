using UnityEngine;
using System.Collections.Generic;

public class GameInfoScrollView : MonoBehaviour
{
    [SerializeField] private GameObject gameInfoPrefab;
    [SerializeField] private RectTransform gameInfoParent;
    private readonly HashSet<GameObject> gameInfoButtons = new();

    private void ConstructGameInfoButtons()
    {
        if (gameInfoPrefab == null) return;

        var gameInfoList = GameInfoManager.RequestGameInfos();
        if (gameInfoList == null || gameInfoList.Count == 0) return;

        for (int i = 0; i < gameInfoList.Count; i++)
        {
            GameObject buttonObject = Instantiate(gameInfoPrefab, gameInfoParent);
            if (!buttonObject.TryGetComponent(out GameInfoButton gameInfoButton) || gameInfoButton == null)
            { Destroy(buttonObject); return; }

            gameInfoButton.SetupGameInfo(gameInfoList[i]);
            gameInfoButtons.Add(buttonObject);
        }
    }

    void OnEnable() { ConstructGameInfoButtons(); }
    void OnDisable()
    {
        foreach (GameObject button in gameInfoButtons) Destroy(button);
        gameInfoButtons.Clear();
    }
}

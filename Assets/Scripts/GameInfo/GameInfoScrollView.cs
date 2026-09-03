using UnityEngine;

public class GameInfoScrollView : MonoBehaviour
{
    [SerializeField] private GameInfoButton gameInfoButton;
    private readonly System.Collections.Generic.HashSet<GameInfoButton> gameInfoButtons = new();

    [SerializeField] private RectTransform parent;

    private void ConstructGameInfoButtons()
    {
        if (gameInfoButton == null) return;

        var gameInfoList = GameInfoManager.RequestGameInfos();
        if (gameInfoList == null || gameInfoList.Count == 0) return;

        for (int i = 0; i < gameInfoList.Count; i++)
        {
            GameInfoButton buttonObject = Instantiate(gameInfoButton, parent);
            buttonObject.SetupGameInfo(gameInfoList[i]);

            gameInfoButtons.Add(buttonObject);
        }
    }

    void OnEnable() { ConstructGameInfoButtons(); }
    void OnDisable()
    {
        foreach (GameInfoButton button in gameInfoButtons) Destroy(button.gameObject);
        gameInfoButtons.Clear();
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class GameInfoButton : MonoBehaviour
{
    private Button button;
    private TextMeshProUGUI gameInfoNameText;

    private GameInfo gameInfo;

    public static event System.Action<GameInfo> Clicked;

    void Awake()
    {
        button = GetComponent<Button>();
        gameInfoNameText = button.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetupGameInfo(GameInfo gameInfo)
    {
        this.gameInfo = gameInfo;
        gameInfoNameText.text = gameInfo.Title;
    }

    public void ShowGameInfo() => Clicked?.Invoke(gameInfo);

    void OnEnable() { button.onClick.AddListener(ShowGameInfo); }
    void OnDisable() { button.onClick.RemoveListener(ShowGameInfo); }
}

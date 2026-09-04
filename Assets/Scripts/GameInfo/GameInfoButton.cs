using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(Button))]
public class GameInfoButton : MonoBehaviour, IPointerClickHandler
{
    private Button button;
    private TextMeshProUGUI text;

    private GameInfo gameInfo;

    public static event System.Action<GameInfo> Clicked;

    void Awake()
    {
        button = GetComponent<Button>();
        text = button.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetupGameInfo(GameInfo gameInfo)
    {
        this.gameInfo = gameInfo;
        text.text = gameInfo.Title;
    }

    public void OnPointerClick(PointerEventData eventData) => Clicked?.Invoke(gameInfo);
}

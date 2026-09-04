using UnityEngine;
using TMPro;

public class GameInfoDisplayView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText, descriptionText;

    private void OnGameInfoDisplayed(GameInfo gameInfo)
    {
        if (gameInfo == null) return;

        if (titleText != null)titleText.text = gameInfo.Title;
        if (descriptionText != null)descriptionText.text = gameInfo.Description;
    }

    void OnEnable() { GameInfoButton.Clicked += OnGameInfoDisplayed; }
    void OnDisable() { GameInfoButton.Clicked -= OnGameInfoDisplayed; }
}

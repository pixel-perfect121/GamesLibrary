using UnityEngine;
using TMPro;

public class GameInfoDisplayView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText, descriptionText;

    private void OnGameInfoDisplayed(GameInfo gameInfo)
    {
        titleText.text = gameInfo.Title;
        descriptionText.text = gameInfo.Description;
    }

    void OnEnable() { GameInfoButton.Clicked += OnGameInfoDisplayed; }
    void OnDisable() { GameInfoButton.Clicked -= OnGameInfoDisplayed; }
}

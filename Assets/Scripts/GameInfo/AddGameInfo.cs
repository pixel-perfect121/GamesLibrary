using UnityEngine;
using TMPro;

public class AddGameInfo : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameField, DescField, ratingField;
    [SerializeField] private UnityEngine.UI.Button submitButton;

    private void AddInfo()
    {
        new GameInfo(nameField.text, DescField.text, int.Parse(ratingField.text));
    }

    void OnEnable()
    {
        submitButton.onClick.AddListener(AddInfo);
    }
}

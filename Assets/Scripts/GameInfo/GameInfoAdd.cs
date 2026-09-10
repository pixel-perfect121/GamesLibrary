using UnityEngine;

public class GameInfoAdd : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_InputField nameField, descriptionField;
    [SerializeField] private UnityEngine.UI.Slider ratingSlider;
    [SerializeField] private UnityEngine.UI.Button submitButton;

    private void OnSubmit()
    {
        if (nameField == null || descriptionField == null || ratingSlider == null) return;

        if (string.IsNullOrWhiteSpace(nameField.text))
        {
            new Notification("System", "Name field cannot be empty");
            return;
        }
        if (string.IsNullOrWhiteSpace(descriptionField.text))
        {
            new Notification("System", "Description field cannot be empty");
            return;
        }

        new GameInfo(nameField.text, descriptionField.text, (int)ratingSlider.value);
        new Notification("System", "New GameInfo entry has successfully materialized.<br>Everybody welcome your brother.");
    }

    void OnEnable() { if (submitButton != null) submitButton.onClick.AddListener(OnSubmit); }
    void OnDisable() { if (submitButton != null) submitButton.onClick.RemoveListener(OnSubmit); }
}

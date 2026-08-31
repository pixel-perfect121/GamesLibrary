using UnityEngine;
using TMPro;

public class TestingAddGameInfo : MonoBehaviour // Inheritance shall once again take place 🗿🗿🗿
{
    [SerializeField] private TMP_InputField nameField, DescField;
    [SerializeField] private UnityEngine.UI.Slider ratingSlider;
    [SerializeField] private UnityEngine.UI.Button submitButton;

    private void AddInfo()
    {
        new GameInfo(nameField.text, DescField.text, Mathf.FloorToInt(ratingSlider.value));
    }

    void OnEnable() { submitButton.onClick.AddListener(AddInfo); }
    void OnDisable() { submitButton.onClick.RemoveListener(AddInfo); }
}

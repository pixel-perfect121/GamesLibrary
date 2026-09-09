using UnityEngine;
using UnityEngine.InputSystem;
using _InputManager;

public sealed class TooltipManager : MonoBehaviour
{
    [Header("UI settings"), SerializeField] private RectTransform container;
    [SerializeField] private TMPro.TextMeshProUGUI titleText, descriptionText;
    [SerializeField] private UnityEngine.UI.Image iconImage;

    [Header("Position settings"), SerializeField] private Vector2 offset;

    private void OnTooltipHovered(Tooltip tooltip, bool isHovered)
    {
        if (container == null || titleText == null || descriptionText == null || iconImage == null) return;
        if (!isHovered) { container.gameObject.SetActive(false); return; }

        titleText.text = tooltip.title;
        descriptionText.text = tooltip.description;
        iconImage.sprite = tooltip.icon;

        container.gameObject.SetActive(true);
    }

    private void UpdatePosition(InputAction.CallbackContext context) => container.position = context.ReadValue<Vector2>() + offset;

    void OnEnable()
    {
        TooltipTrigger.Hovered += OnTooltipHovered;
        InputManager.Input.Mouse.Move.performed += UpdatePosition;
    }
    void OnDisable()
    {
        TooltipTrigger.Hovered -= OnTooltipHovered;
        InputManager.Input.Mouse.Move.performed -= UpdatePosition;
    }
}

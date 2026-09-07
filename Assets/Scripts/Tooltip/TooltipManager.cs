using UnityEngine;

public sealed class TooltipManager : MonoBehaviour
{
    [SerializeField, Header("Container")] private GameObject container;

    [Header("Tooltip requirements")]
    [SerializeField] private TMPro.TextMeshProUGUI titleText;
    [SerializeField] private TMPro.TextMeshProUGUI descriptionText;
    [SerializeField] private UnityEngine.UI.Image iconImage;

    private void OnTooltipHovered(Tooltip tooltip, bool isHovered)
    {
        if (titleText == null || descriptionText == null || iconImage == null) return;

        titleText.text = tooltip.title;
        descriptionText.text = tooltip.description;

        iconImage.sprite = null;
        if (tooltip.sprite != null) iconImage.sprite = tooltip.sprite;

        container.SetActive(isHovered);
    }

    void OnEnable() { TooltipTrigger.Hovered += OnTooltipHovered; }
    void OnDisable() { TooltipTrigger.Hovered -= OnTooltipHovered; }
}

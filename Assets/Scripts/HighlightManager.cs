using UnityEngine;

public class HighlightManager : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image image;
    [SerializeField] private Vector2 sizeOffset;

    private void OnHighlighted(RectTransform rect, bool isHighlighted)
    {
        if (image == null) return;

        image.rectTransform.sizeDelta = rect.rect.size + sizeOffset;
        image.rectTransform.position = rect.position;
        image.gameObject.SetActive(isHighlighted);
    }

    void OnEnable() { HighlightTrigger.Highlighted += OnHighlighted; }
    void OnDisable() { HighlightTrigger.Highlighted -= OnHighlighted; }
}

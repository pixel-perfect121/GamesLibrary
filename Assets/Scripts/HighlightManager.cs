using UnityEngine;

public class HighlightManager : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image highlightImage, selectionImage;
    [SerializeField] private Vector2 sizeOffset;

    private void OnHighlighted(RectTransform rect, bool isHighlighted)
    {
        if (highlightImage == null) return;

        highlightImage.rectTransform.sizeDelta = rect.rect.size + sizeOffset;
        highlightImage.rectTransform.position = rect.position;

        highlightImage.gameObject.SetActive(isHighlighted);
    }
    private void OnSelected(RectTransform rect)
    {
        if (selectionImage == null) return;

        selectionImage.rectTransform.sizeDelta = rect.rect.size + sizeOffset;
        selectionImage.rectTransform.position = rect.position;

        selectionImage.gameObject.SetActive(true);
    }

    void OnEnable()
    {
        HighlightTrigger.Highlighted += OnHighlighted;
        HighlightTrigger.Selected += OnSelected;
    }
    void OnDisable()
    {
        HighlightTrigger.Highlighted -= OnHighlighted;
        HighlightTrigger.Selected -= OnSelected;
    }
}

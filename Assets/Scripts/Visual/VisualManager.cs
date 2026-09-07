using UnityEngine;

public class VisualManager : MonoBehaviour
{
    [Header("Image transforms")]
    [SerializeField] private RectTransform highlightRect;
    [SerializeField] private RectTransform clickRect;

    [Header("Animation settings")]
    [SerializeField] private Vector2 sizeOffset;
    [SerializeField, Range(0.01f, 1.25f)] private float animationDuration;
    [SerializeField] private AnimationCurve curve;

    private Coroutine highlightRoutine, clickRoutine;

    private void OnHighlighted(RectTransform rect, bool isHighlighted)
    {
        if (highlightRect == null) return;

        highlightRect.position = rect.position;
        highlightRect.sizeDelta = rect.rect.size;
        highlightRect.gameObject.SetActive(isHighlighted);

        if (highlightRoutine != null) StopCoroutine(highlightRoutine);
        highlightRoutine = StartCoroutine(SmoothPosition(highlightRect, rect.position, rect.rect.size + sizeOffset));
    }
    private void OnClicked(RectTransform rect)
    {
        if (clickRect == null) return;

        clickRect.position = rect.position;
        clickRect.sizeDelta = rect.rect.size;
        clickRect.gameObject.SetActive(true);

        if (clickRoutine != null) StopCoroutine(clickRoutine);
        clickRoutine = StartCoroutine(SmoothPosition(clickRect, rect.position, rect.rect.size + sizeOffset));
    }

    private System.Collections.IEnumerator SmoothPosition(RectTransform rect, Vector2 targetPosition, Vector2 targetSize)
    {
        Vector2 startPosition = rect.position, startSize = rect.sizeDelta;

        float elapsed = 0f;
        while (elapsed <= 1f)
        {
            float evaluatedTime = curve.Evaluate(elapsed);
            rect.position = Vector2.Lerp(startPosition, targetPosition, evaluatedTime);
            rect.sizeDelta = Vector2.Lerp(startSize, targetSize, evaluatedTime);

            elapsed += Time.deltaTime / animationDuration;
            yield return null;
        }
        rect.position = targetPosition;
        rect.sizeDelta = targetSize;
    }

    void OnEnable()
    {
        VisualTrigger.Highlighted += OnHighlighted;
        VisualTrigger.Clicked += OnClicked;
    }
    void OnDisable()
    {
        VisualTrigger.Highlighted -= OnHighlighted;
        VisualTrigger.Clicked -= OnClicked;
    }
}

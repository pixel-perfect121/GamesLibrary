using UnityEngine;
using System.Collections;

public class HighlightManager : MonoBehaviour
{
    [Header("Transforms")]
    [SerializeField] private RectTransform highlightRect;
    [SerializeField] private RectTransform selectionRect;

    [Header("Animation settings")]
    [SerializeField] private Vector2 sizeOffset;
    [SerializeField, Range(0.01f, 2.25f)] private float animationDuration;
    [SerializeField] private AnimationCurve curve;

    private Coroutine highlightRoutine, selectionRoutine;

    private void OnHighlighted(RectTransform rect, bool isHighlighted)
    {
        if (highlightRect == null) return;

        highlightRect.gameObject.SetActive(isHighlighted);

        if (highlightRoutine != null) StopCoroutine(highlightRoutine);
        highlightRoutine = StartCoroutine(SmoothMove(highlightRect, rect.position, rect.rect.size + sizeOffset));
    }
    private void OnSelected(RectTransform rect)
    {
        if (selectionRect == null) return;

        selectionRect.gameObject.SetActive(true);

        if (selectionRoutine != null) StopCoroutine(selectionRoutine);
        selectionRoutine = StartCoroutine(SmoothMove(selectionRect, rect.position, rect.rect.size + sizeOffset));
    }

    private IEnumerator SmoothMove(RectTransform rect, Vector2 targetPosition, Vector2 targetSize)
    {
        Vector2 startPosition = rect.position, startSize = rect.sizeDelta;

        float elapsed = 0f;
        while (elapsed < 1f)
        {
            float evaluatedTime = curve.Evaluate(elapsed);
            rect.position = Vector2.Lerp(startPosition, targetPosition, evaluatedTime);
            rect.sizeDelta = Vector2.Lerp(startSize, targetSize, evaluatedTime);

            elapsed += Time.deltaTime / animationDuration;
            yield return null;
        }
        rect.position = targetPosition; rect.sizeDelta = targetSize;
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

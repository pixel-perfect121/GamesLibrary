using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class HighlightTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rect;
    public static event System.Action<RectTransform, bool> Highlighted;

    void Awake() { rect = GetComponent<RectTransform>(); }

    public void OnPointerEnter(PointerEventData eventData) => Highlighted?.Invoke(rect, true);
    public void OnPointerExit(PointerEventData eventData) => Highlighted?.Invoke(rect, false);
}

using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class VisualTrigger : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    private RectTransform rect;

    public static event System.Action<RectTransform, bool> Highlighted;
    public static event System.Action<RectTransform> Clicked;

    void Awake() { rect = GetComponent<RectTransform>(); }

    public void OnPointerEnter(PointerEventData eventData) => Highlighted?.Invoke(rect, true);
    public void OnPointerExit(PointerEventData eventData) => Highlighted?.Invoke(rect, false);
    public void OnPointerClick(PointerEventData eventData) => Clicked?.Invoke(rect);
}

using UnityEngine;
using UnityEngine.EventSystems;

public sealed class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Tooltip tooltip;

    public static event System.Action<Tooltip, bool> Hovered;

    public void OnPointerEnter(PointerEventData eventData) => Hovered?.Invoke(tooltip, true);
    public void OnPointerExit(PointerEventData eventData) => Hovered?.Invoke(tooltip, false);
}

[System.Serializable] public struct Tooltip { public string title, description; public Sprite icon; }

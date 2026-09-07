using UnityEngine;
using UnityEngine.EventSystems;

public sealed class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Tooltip tooltipSO;

    public static event System.Action<Tooltip, bool> Hovered;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tooltipSO == null) return;

        Hovered?.Invoke(tooltipSO, true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltipSO == null) return;

        Hovered?.Invoke(tooltipSO, false);
    }
}

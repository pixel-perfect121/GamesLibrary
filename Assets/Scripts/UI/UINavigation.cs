using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Selectable))]
public class UINavigation : MonoBehaviour, IMoveHandler
{
    private Selectable selectable;

    void Awake() { selectable = GetComponent<Selectable>(); }

    public void OnMove(AxisEventData eventData)
    {
        Selectable targetSelectable = eventData.moveDir switch
        {
            MoveDirection.Up => selectable.FindSelectableOnUp(),
            MoveDirection.Down => selectable.FindSelectableOnDown(),
            MoveDirection.Left => selectable.FindSelectableOnLeft(),
            MoveDirection.Right => selectable.FindSelectableOnRight(),
            MoveDirection.None => null, _ => null
        };

        if (targetSelectable != null) Debug.Log(selectable.name, selectable.gameObject);
    }
}

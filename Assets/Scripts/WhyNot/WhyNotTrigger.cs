using UnityEngine;
using UnityEngine.EventSystems;

public class WhyNotTrigger : MonoBehaviour, IPointerClickHandler
{
    public static event System.Action<GameObject> ObjectAdded;

    public void OnPointerClick(PointerEventData eventData) => ObjectAdded?.Invoke(gameObject);
}

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle), typeof(RectTransform))]
public class Testing : MonoBehaviour, IPointerDownHandler
{
    private RectTransform rect;
    private Toggle toggle;

    private Bounds screenBounds;

    public static event System.Action<bool> Toggled;

    void Awake()
    {
        toggle = GetComponent<Toggle>();
        rect = GetComponent<RectTransform>();

        screenBounds = new(rect.position, new(Screen.width, Screen.height, 0f));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        toggle.isOn = !toggle.isOn;
        Toggled?.Invoke(toggle.isOn);

        TeleportRandomly();
    }

    private void TeleportRandomly()
    {
        float x = Random.Range(screenBounds.min.x, screenBounds.max.x),
              y = Random.Range(screenBounds.min.y, screenBounds.max.y);

        rect.position = new(x, y, 0f);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(screenBounds.center, screenBounds.size);
    }
}

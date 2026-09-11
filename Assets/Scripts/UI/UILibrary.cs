using UnityEngine;
using UnityEngine.EventSystems;
using _UIManager;

[RequireComponent(typeof(EventSystem)), DefaultExecutionOrder(-300)]
public class UILibrary : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadePanel;
    [SerializeField] private System.Collections.Generic.List<Panel> panels = new();
    private EventSystem currentEventSystem;

    private PanelType currentPanel;

    void Awake()
    {
        currentEventSystem = GetComponent<EventSystem>();

        foreach (Panel panel in panels)
        {
            if (panel == null) continue;
            if (!panel.canvasGroup.gameObject.activeInHierarchy) continue;

            currentPanel = panel.panelType; break;
        }
    }

    private void OnPanelTransitioned(PanelType targetPanel)
    {
        if (targetPanel == currentPanel) return;

        Panel target = UIManager.GetPanel(targetPanel);
        StartCoroutine(UIManager.DipToColor(currentPanel, targetPanel, 5f, 1f, fadePanel, DeselectButtons, () =>
        { if (target.defaultButton != null) SelectButton(target.defaultButton); }));
        currentPanel = target.panelType;
    }

    private void SelectButton(GameObject button) => currentEventSystem.SetSelectedGameObject(button);
    private void DeselectButtons() => currentEventSystem.SetSelectedGameObject(null);

    void OnEnable()
    {
        foreach (Panel panel in panels) UIManager.Register(panel);

        GoToPanel.PanelTransition += OnPanelTransitioned;
    }
    void OnDisable()
    {
        foreach (Panel panel in panels) UIManager.Unregister(panel);

        GoToPanel.PanelTransition -= OnPanelTransitioned;
    }
}

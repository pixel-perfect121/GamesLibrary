using UnityEngine;
using UnityEngine.EventSystems;
using _UIManager;

[DefaultExecutionOrder(-300), RequireComponent(typeof(EventSystem))]
public sealed class UILibrary : MonoBehaviour
{
    [SerializeField] private System.Collections.Generic.List<Panel> panels = new();

    private EventSystem currentEventSystem;
    private PanelType currentPanel;

    void Awake()
    {
        currentEventSystem = GetComponent<EventSystem>();
    }

    void Start()
    {
        foreach (Panel panel in panels)
        {
            if (panel == null) continue;
            if (!panel.canvasGroup.gameObject.activeInHierarchy) continue;

            currentPanel = panel.panelType;
        }
    }

    private void OnPanelTransitioned(PanelType targetPanel)
    {
        if (targetPanel == currentPanel) return;

        Panel panel = UIManager.GetPanel(targetPanel);
        if (panel == null) return;

        currentEventSystem.SetSelectedGameObject(null);

        UIManager.Switch(panel.panelType);
        if (panel.defaultButton != null) currentEventSystem.SetSelectedGameObject(panel.defaultButton);
        currentPanel = panel.panelType;
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        foreach (Panel panel in panels)
        {
            if (panel == null)
            {
                Debug.LogWarning("Some panels were not initialized", this);
                continue;
            }

            panel.SetName();
        }
    }
#endif

    void OnEnable()
    {
        foreach (Panel panel in panels) UIManager.Register(panel);

        GoToPanel.PanelTransitioned += OnPanelTransitioned;
    }
    void OnDisable()
    {
        foreach (Panel panel in panels) UIManager.Unregister(panel);

        GoToPanel.PanelTransitioned -= OnPanelTransitioned;
    }
}

using UnityEngine;
using _UIManager;

[DefaultExecutionOrder(-300)]
public sealed class UILibrary : MonoBehaviour
{
    [SerializeField] private System.Collections.Generic.List<Panel> panels = new();

    void Start()
    {
        UIManager.Initialize(PanelType.MainMenu);
    }

    private void OnPanelTransitioned(PanelType targetPanel) => UIManager.Switch(targetPanel);

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

using UnityEngine;
using UnityEngine.UI;
using _UIManager;

public class GoToPanel : MonoBehaviour
{
    [SerializeField] private PanelType targetPanel;
    private Button button;

    public static event System.Action<PanelType> PanelTransitioned;

    void Awake() { button = GetComponent<Button>(); }

    private void TransitionPanel() => PanelTransitioned?.Invoke(targetPanel);

    void OnEnable() { if (button != null) button.onClick.AddListener(TransitionPanel); }
    void OnDisable() { if (button != null) button.onClick.RemoveListener(TransitionPanel); }
}

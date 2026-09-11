using UnityEngine;
using UnityEngine.UI;
using _UIManager;

[RequireComponent(typeof(Button))]
public class GoToPanel : MonoBehaviour
{
    [SerializeField] private PanelType targetPanel;
    private Button button;

    public static event System.Action<PanelType> PanelTransition;

    void Awake() { button = GetComponent<Button>(); }

    private void ChangePanel() => PanelTransition?.Invoke(targetPanel);

    void OnEnable() { button.onClick.AddListener(ChangePanel); }
    void OnDisable() { button.onClick.RemoveListener(ChangePanel); }
}

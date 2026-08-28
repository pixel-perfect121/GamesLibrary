using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum ButtonFunction { Enter, Settings, Exit, }

[RequireComponent(typeof(Button))]
public sealed class ButtonFunctionality : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private ButtonFunction function;
    private Button button;

    void Awake() { button = GetComponent<Button>(); }

    public static event System.Action<ButtonFunction> Entered, Clicked, Exited;
    public static event System.Action<Button> ButtonEntered, ButtonClicked, ButtonExited;

    private ButtonFunctionality() { }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Entered?.Invoke(function);
        ButtonEntered?.Invoke(button);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Clicked?.Invoke(function);
        ButtonClicked?.Invoke(button);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Exited?.Invoke(function);
        ButtonExited?.Invoke(button);
    }
}

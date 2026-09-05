using UnityEngine;
using UnityEngine.InputSystem;
using _InputManager;

public class WhyNot : MonoBehaviour
{
    private readonly System.Collections.Generic.Queue<GameObject> collection = new();

    private void OnObjectAdded(GameObject @object)
    {
        if (collection.Contains(@object)) return;

        collection.Enqueue(@object);
    }
    private void DestroyObjects(InputAction.CallbackContext context)
    {
        if (collection.Count == 0) return;

        Destroy(collection.Dequeue());
    }

    void OnEnable()
    {
        WhyNotTrigger.ObjectAdded += OnObjectAdded;
        InputManager.Input.WhyNot.Destroy.performed += DestroyObjects;
    }
    void OnDisable()
    {
        WhyNotTrigger.ObjectAdded -= OnObjectAdded;
        InputManager.Input.WhyNot.Destroy.performed -= DestroyObjects;
    }
}

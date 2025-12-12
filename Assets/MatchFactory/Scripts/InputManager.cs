using UnityEngine;
using UnityEngine.InputSystem;

using System;


public class InputManager : MonoBehaviour
{
    private InputAction clickAction;


    public static Action<Item> itemClicked;

    [Header("Settings")]
    private Item currentItem;

    private void Awake()
    {
        // Create a simple button action that listens for left mouse button and primary touch press
        clickAction = new InputAction("Click", InputActionType.Button);
        clickAction.AddBinding("<Mouse>/leftButton");
        clickAction.AddBinding("<Touchscreen>/primaryTouch/press");
        clickAction.performed += OnClickPerformed;
    }

    private void OnEnable()
    {
        clickAction?.Enable();
    }

    private void OnDisable()
    {
        if (clickAction != null)
        {
            clickAction.performed -= OnClickPerformed;
            clickAction.Disable();
        }
    }

    private void OnClickPerformed(InputAction.CallbackContext context)
    {
        HandlePointerDown();
    }

    private void HandlePointerDown()
    {
        Vector2 pointerPos = Vector2.zero;
        // Prefer the generic Pointer (works for mouse, pen, touch). Fall back to Touchscreen or Mouse.
        if (Pointer.current != null)
        {
            pointerPos = Pointer.current.position.ReadValue();
        }
        else if (Touchscreen.current != null && Touchscreen.current.primaryTouch != null)
        {
            pointerPos = Touchscreen.current.primaryTouch.position.ReadValue();
        }
        else if (Mouse.current != null)
        {
            pointerPos = Mouse.current.position.ReadValue();
        }
        Camera cam = Camera.main;
        if (cam == null)
        {
            return;
        }
        Ray ray = cam.ScreenPointToRay(pointerPos);
        Physics.Raycast(ray, out RaycastHit hitInfo, 100);

        if (hitInfo.collider == null)
        {
            return;
        }



        Item item = null;
        // First try the exact collider's gameobject
        if (!hitInfo.collider.TryGetComponent<Item>(out item))
        {
            // If not found, try the parents (common when collider is on a child object)
            item = hitInfo.collider.GetComponentInParent<Item>();
            if (item == null)
            {
                return;
            }
        }

        Debug.Log("Clicked on item: " + item.gameObject.name);
        itemClicked?.Invoke(item);

    }
}

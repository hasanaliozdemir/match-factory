using UnityEngine;
using UnityEngine.InputSystem;

using System;
using Unity.InferenceEngine;


public class InputManager : MonoBehaviour
{
    private InputAction clickAction;

    public static Action<Item> itemClicked;
    public static Action<PowerUp> powerupClicked;

    [Header("Settings")]
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private LayerMask powerUpLayerMask;
    [SerializeField] private bool showRaycastDebug = false;
    private Item currentItem;
    private bool isDragging = false;

    private void Awake()
    {
        // Create a simple button action that listens for left mouse button and primary touch press
        clickAction = new InputAction("Click", InputActionType.Button);
        clickAction.AddBinding("<Mouse>/leftButton");
        clickAction.AddBinding("<Touchscreen>/primaryTouch/press");
        // Use started/canceled so we can track dragging and release
        clickAction.started += OnClickStarted;
        clickAction.canceled += OnClickCanceled;
    }

    private void OnEnable()
    {
        clickAction?.Enable();
    }

    private void OnDisable()
    {
        if (clickAction != null)
        {
            clickAction.started -= OnClickStarted;
            clickAction.canceled -= OnClickCanceled;
            clickAction.Disable();
        }
    }


    private void HandleControls()
    {
        if (!isDragging)
            return;

        Vector2 pointerPos = GetPointerPosition();
        Item hit = GetItemUnderPointer(pointerPos);
        if (hit != currentItem)
        {
            // Deselect the old item
            if (currentItem != null)
                currentItem.Deselect();

            // Select the new item
            currentItem = hit;
            if (currentItem != null)
            {
                currentItem.Select(outlineMaterial);
                Debug.Log("Dragging over item: " + currentItem.gameObject.name);
            }
        }
    }

    private void Update()
    {
        if (GameManager.instance.IsGame)
        {
            HandleControls();
        }
    }

    private void OnClickStarted(InputAction.CallbackContext context)
    {
        HandleClickDown();
        isDragging = true;
        currentItem = null;
        // do an initial update immediately
        Vector2 pointerPos = GetPointerPosition();
        currentItem = GetItemUnderPointer(pointerPos);
        if (currentItem != null)
        {
            currentItem.Select(outlineMaterial);
            Debug.Log("Started drag on item: " + currentItem.gameObject.name);
        }
    }

    private void OnClickCanceled(InputAction.CallbackContext context)
    {
        // pointer released
        isDragging = false;
        // Deselect the currently selected item
        if (currentItem != null)
            currentItem.Deselect();

        // final check at release position
        Vector2 pointerPos = GetPointerPosition();
        Item releasedOver = GetItemUnderPointer(pointerPos);
        // Use the latest collided item as `currentItem` (as requested)
        currentItem = releasedOver;
        if (currentItem != null)
        {
            Debug.Log("Released over item: " + currentItem.gameObject.name);
            itemClicked?.Invoke(currentItem);
        }
        currentItem = null;
    }

    private void HandleClickDown()
    {
        Physics.Raycast(Camera.main.ScreenPointToRay(GetPointerPosition()), out RaycastHit hitInfo, 100, powerUpLayerMask);

        if (hitInfo.collider == null)
        {
            return;
        }

        powerupClicked?.Invoke(hitInfo.collider.GetComponent<PowerUp>());
    }

    private Vector2 GetPointerPosition()
    {
        Vector2 pointerPos = Vector2.zero;
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
        return pointerPos;
    }

    private Item GetItemUnderPointer(Vector2 pointerPos)
    {
        Camera cam = Camera.main;
        if (cam == null)
            return null;
        Ray ray = cam.ScreenPointToRay(pointerPos);

        if (!Physics.Raycast(ray, out RaycastHit hitInfo, 100))
        {
            if (showRaycastDebug)
                Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 0.1f);
            return null;
        }

        if (showRaycastDebug)
        {
            Debug.DrawRay(ray.origin, ray.direction * hitInfo.distance, Color.green, 0.1f);
            Debug.DrawRay(hitInfo.point, Vector3.up * 0.5f, Color.yellow, 0.1f);
        }



        if (hitInfo.collider == null)
        {
            return null;
        }

        if (hitInfo.collider.transform.parent == null)
        {
            return null;
        }

        Item item = null;
        if (!hitInfo.collider.transform.parent.TryGetComponent<Item>(out item))
        {
            item = hitInfo.collider.GetComponentInParent<Item>();
        }
        return item;
    }
}

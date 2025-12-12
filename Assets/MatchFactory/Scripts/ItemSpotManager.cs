using UnityEngine;

public class ItemSpotManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform itemSpot;

    [Header("Settings")]
    [SerializeField] private Vector3 itemLocalPositionOnSpot;
    [SerializeField] private Vector3 itemLocalScaleOnSpot;

    void Awake()
    {
        InputManager.itemClicked += OnItemClicked;
    }

    void OnDestroy()
    {
        InputManager.itemClicked -= OnItemClicked;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnItemClicked(Item item)
    {
        // 1. Turn the item as a child of item spot

        item.transform.SetParent(itemSpot);

        // 2. Scale the item down, set its position and rotation to zero

        item.transform.localPosition = itemLocalPositionOnSpot;
        item.transform.localScale = itemLocalScaleOnSpot;
        item.transform.localRotation = Quaternion.identity;
        // 3. Disable item's shadow

        item.DisableShadow();

        // 4. Disable item's collider/Physics

        item.DisablePhysics();
    }
}

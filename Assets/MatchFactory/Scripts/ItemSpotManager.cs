using UnityEngine;

public class ItemSpotManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform itemSpotsParent;
    private ItemSpot[] spots;

    [Header("Settings")]
    [SerializeField] private Vector3 itemLocalPositionOnSpot;
    [SerializeField] private Vector3 itemLocalScaleOnSpot;

    void Awake()
    {
        InputManager.itemClicked += OnItemClicked;

        StoreSpots();
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

        if (!IsFreeSpotAvailable())
        {
            Debug.Log("No free item spot available!");
            return;
        }

        HandleItemClicked(item);


    }

    private void HandleItemClicked(Item item)
    {
        MoveItemtoFirstFreeSpot(item);
    }

    private void MoveItemtoFirstFreeSpot(Item item)
    {
        ItemSpot targetSpot = GetFreeSpot();

        if (targetSpot == null)
        {
            Debug.LogError("No free item spot found! !! Logic error.");
            return;
        }

        // 1. Turn the item as a child of item spot

        targetSpot.Populate(item);

        // 2. Scale the item down, set its position and rotation to zero

        item.transform.localPosition = itemLocalPositionOnSpot;
        item.transform.localScale = itemLocalScaleOnSpot;
        item.transform.localRotation = Quaternion.identity;
        // 3. Disable item's shadow

        item.DisableShadow();

        // 4. Disable item's collider/Physics

        item.DisablePhysics();
    }

    private void StoreSpots()
    {
        int spotCount = itemSpotsParent.childCount;
        spots = new ItemSpot[spotCount];
        for (int i = 0; i < spotCount; i++)
        {
            spots[i] = itemSpotsParent.GetChild(i).GetComponent<ItemSpot>();
        }
    }

    private ItemSpot GetFreeSpot()
    {
        for (int i = 0; i < spots.Length; i++)
        {
            if (spots[i].IsEmpty())
            {
                return spots[i];
            }
        }
        return null;
    }

    private bool IsFreeSpotAvailable()
    {
        for (int i = 0; i < spots.Length; i++)
        {
            if (spots[i].IsEmpty())
            {
                return true;
            }
        }
        return false;
    }
}

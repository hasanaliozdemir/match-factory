using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpotManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform itemSpotsParent;
    private ItemSpot[] spots;



    [Header("Data")]
    private Dictionary<string, ItemMergeData> itemMergeDataDictionary = new Dictionary<string, ItemMergeData>();


    [Header("Settings")]
    [SerializeField] private Vector3 itemLocalPositionOnSpot;
    [SerializeField] private Vector3 itemLocalScaleOnSpot;
    private bool isBusy;

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

        if (isBusy)
        {
            Debug.Log("ItemSpotManager is busy!");
            return;
        }

        if (!IsFreeSpotAvailable())
        {
            Debug.Log("No free item spot available!");
            return;
        }

        isBusy = true;

        HandleItemClicked(item);


    }

    private void HandleItemClicked(Item item)
    {
        if (itemMergeDataDictionary.ContainsKey(item.name))
        {
            HandleItemMergeDataFound(item);

        }
        else
        {
            MoveItemtoFirstFreeSpot(item);
        }


    }

    private void HandleItemMergeDataFound(Item item)
    {
        ItemSpot idealSpot = GetIdealSpotFor(item);

        itemMergeDataDictionary[item.name].AddItem(item);


        TryMoveItemToIdealSpot(item, idealSpot);
    }

    private void TryMoveItemToIdealSpot(Item item, ItemSpot targetSpot)
    {
        if (!targetSpot.IsEmpty())
        {
            HandleIdealSpotFull(item, targetSpot);
            return;
        }

        MoveItemtoSpot(item, targetSpot);
    }

    private void MoveItemtoSpot(Item item, ItemSpot targetSpot, bool checkForMerge = true)
    {
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

        HandleItemReachedSpot(item, checkForMerge);
    }

    private void HandleItemReachedSpot(Item item, bool checkForMerge = true)
    {

        if (!checkForMerge)
        {
            return;
        }

        if (itemMergeDataDictionary[item.name].CanMergeItems())
        {
            MergeItems(itemMergeDataDictionary[item.name]);
        }
        else
        {
            CheckForGameOver();
        }
    }

    private void MergeItems(ItemMergeData itemMergeData)
    {
        List<Item> items = itemMergeData.items;

        // Remove the item merge data from dictionary
        itemMergeDataDictionary.Remove(itemMergeData.itemName);

        for (int i = 0; i < items.Count; i++)
        {
            items[i].Spot.Clear();
            Destroy(items[i].gameObject);
        }

        MoveAllITemsToTheLeft();

    }

    private void MoveAllITemsToTheLeft()
    {
        for (int i = 3; i < spots.Length; i++)
        {
            ItemSpot spot = spots[i];
            if (spot.IsEmpty())
            {
                continue;
            }
            Item item = spot.Item;

            spot.Clear();

            ItemSpot targetSpot = spots[i - 3];
            if (!targetSpot.IsEmpty())
            {
                Debug.LogError($"Logic error: target spot {targetSpot.name} is not empty when moving items to the left.");
                isBusy = false;
                continue;
            }

            spot.Clear();

            MoveItemtoSpot(item, targetSpot, false);

        }
        HandleAllItemsMovedToTheLeft();
    }

    private void HandleAllItemsMovedToTheLeft()
    {
        isBusy = false;
    }

    private void HandleIdealSpotFull(Item item, ItemSpot idealSpot)
    {
        MoveAllITemsToTheRightFrom(idealSpot, item);
    }

    private void MoveAllITemsToTheRightFrom(ItemSpot idealSpot, Item itemToPlace)
    {
        int spotIndex = idealSpot.transform.GetSiblingIndex(); // 2

        for (int i = spots.Length - 2; i >= spotIndex; i--) // 5,4,3,2
        {
            ItemSpot spot = spots[i];
            Debug.Log("Checking spot at index: " + i);
            if (spot.IsEmpty())
            {
                Debug.Log("Spot at index " + i + " is empty, skipping.");
                continue;
            }
            Item item = spot.Item;

            spot.Clear();

            ItemSpot targetSpot = spots[i + 1];
            Debug.Log("Target spot index: " + (i + 1));
            if (!targetSpot.IsEmpty())
            {
                Debug.LogError("Logic error: target spot is not empty when moving items to the right.");
                isBusy = false;
                continue;
            }
            Debug.Log("Moving item " + item.name + " from spot " + i + " to spot " + (i + 1));
            MoveItemtoSpot(item, targetSpot, false);
        }
        MoveItemtoSpot(itemToPlace, idealSpot);
    }

    private ItemSpot GetIdealSpotFor(Item item)
    {
        List<Item> items = itemMergeDataDictionary[item.name].items;
        List<ItemSpot> itemSpots = new List<ItemSpot>();

        for (int i = 0; i < items.Count; i++)
        {
            itemSpots.Add(items[i].Spot);
        }

        // If only 1 spot, we should return next spot
        if (itemSpots.Count >= 2)
        {
            itemSpots.Sort((a, b) => b.transform.GetSiblingIndex().CompareTo(a.transform.GetSiblingIndex()));
        }

        int idealSpotIndex = itemSpots[0].transform.GetSiblingIndex() + 1;

        return spots[idealSpotIndex];
    }

    private void MoveItemtoFirstFreeSpot(Item item)
    {

        ItemSpot targetSpot = GetFreeSpot();

        if (targetSpot == null)
        {
            Debug.LogError("No free item spot found! !! Logic error.");
            return;
        }

        CreateItemMergeData(item);

        MoveItemtoSpot(item, targetSpot);
    }

    private void HandleFirstItemReachedSpot(Item item)
    {
        CheckForGameOver();
    }

    private void CheckForGameOver()
    {
        if (GetFreeSpot() == null)
        {
            Debug.Log("Game Over! No free item spots left.");
        }
        isBusy = false;
    }

    private void CreateItemMergeData(Item item)
    {
        itemMergeDataDictionary.Add(item.name, new ItemMergeData(item));
        Debug.Log("Created ItemMergeData for item: " + item.name);
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

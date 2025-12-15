using System;
using System.Collections.Generic;
using System.Linq;
using MatchFactory.Scripts.Enums;
using UnityEngine;
using UnityEngine.XR;

public class ItemSpotManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform itemSpotsParent;
    private ItemSpot[] spots;



    [Header("Data")]
    private Dictionary<ItemEnum, ItemMergeData> itemMergeDataDictionary = new Dictionary<ItemEnum, ItemMergeData>();


    [Header("Settings")]
    [SerializeField] private Vector3 itemLocalPositionOnSpot;
    [SerializeField] private Vector3 itemLocalScaleOnSpot;


    [Header("Animation Settings")]
    [SerializeField] private float animationDuration = 0.15f;
    [SerializeField] private LeanTweenType animationEaseType = LeanTweenType.easeInOutSine;

    [Header("Actions")]
    public static Action<List<Item>> mergeStarted;

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
        if (itemMergeDataDictionary.ContainsKey(item.Type))
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

        itemMergeDataDictionary[item.Type].AddItem(item);


        TryMoveItemToIdealSpot(item, idealSpot);
    }

    private void TryMoveItemToIdealSpot(Item item, ItemSpot targetSpot)
    {
        if (!targetSpot.IsEmpty())
        {
            HandleIdealSpotFull(item, targetSpot);
            return;
        }

        MoveItemToSpot(item, targetSpot, () => HandleItemReachedSpot(item));
    }

    private void MoveItemToSpot(Item item, ItemSpot targetSpot, Action completeCallback)
    {
        // 1. Turn the item as a child of item spot

        targetSpot.Populate(item);

        // 2. Scale the item down, set its position and rotation to zero

        // item.transform.localPosition = itemLocalPositionOnSpot;
        // item.transform.localScale = itemLocalScaleOnSpot;
        // item.transform.localRotation = Quaternion.identity;

        // Which object, where, how fast
        LeanTween.moveLocal(item.gameObject, itemLocalPositionOnSpot, animationDuration).setEase(animationEaseType);
        LeanTween.scale(item.gameObject, itemLocalScaleOnSpot, animationDuration).setEase(animationEaseType);
        LeanTween.rotateLocal(item.gameObject, Vector3.zero, animationDuration)
        .setEase(animationEaseType)
        .setOnComplete(completeCallback);


        // 3. Disable item's shadow
        item.DisableShadow();

        // 4. Disable item's collider/Physics

        item.DisablePhysics();


    }

    private void HandleItemReachedSpot(Item item, bool checkForMerge = true)
    {

        item.Spot.BumpDown();

        if (!checkForMerge)
        {
            return;
        }

        if (itemMergeDataDictionary[item.Type].CanMergeItems())
        {
            MergeItems(itemMergeDataDictionary[item.Type]);
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
        itemMergeDataDictionary.Remove(itemMergeData.itemType);

        foreach (var t in items)
        {
            t.Spot.Clear();
            // Destroy(t.gameObject);


        }

        if (itemMergeDataDictionary.Count <= 0)
        {
            isBusy = false;
        }
        else
        {
            MoveAllITemsToTheLeft(HandleAllItemsMovedToTheLeft);
        }

        mergeStarted?.Invoke(items);

    }

    private void MoveAllITemsToTheLeft(Action completeCallback)
    {
        bool callBackTriggered = false;
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

            completeCallback += () => HandleItemReachedSpot(item, false);

            MoveItemToSpot(item, targetSpot, completeCallback);
            callBackTriggered = true;
        }
        if (!callBackTriggered)
        {
            completeCallback?.Invoke();
        }
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
            MoveItemToSpot(item, targetSpot, () => HandleItemReachedSpot(item, false));
        }
        MoveItemToSpot(itemToPlace, idealSpot, () => HandleItemReachedSpot(itemToPlace));
    }

    private ItemSpot GetIdealSpotFor(Item item)
    {
        List<Item> items = itemMergeDataDictionary[item.Type].items;
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

        MoveItemToSpot(item, targetSpot, () => HandleFirstItemReachedSpot(item));
    }

    private void HandleFirstItemReachedSpot(Item item)
    {
        item.Spot.BumpDown();

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
        itemMergeDataDictionary.Add(item.Type, new ItemMergeData(item));
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
        return spots.FirstOrDefault(t => t.IsEmpty());
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

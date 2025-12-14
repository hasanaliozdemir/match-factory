using System.Collections.Generic;
using MatchFactory.Scripts.Enums;
using UnityEngine;

public struct ItemMergeData
{
    public ItemEnum itemType;
    public List<Item> items;

    public ItemMergeData(Item firstItem)
    {
        itemType = firstItem.Type;
        items = new List<Item> { firstItem };
    }

    public void AddItem(Item item)
    {
        items.Add(item);
    }

    public bool CanMergeItems()
    {
        return items.Count >= 3;
    }
}

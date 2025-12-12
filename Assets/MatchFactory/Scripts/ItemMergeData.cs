using System.Collections.Generic;
using UnityEngine;

public struct ItemMergeData
{
    public string itemName;
    public List<Item> items;

    public ItemMergeData(Item firstItem)
    {
        itemName = firstItem.name;
        items = new List<Item>();
        items.Add(firstItem);

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

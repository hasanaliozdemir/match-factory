using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{

    [Header("Actions")]
    public static Action<Item> itemPickedUp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    [Button]
    private void VacuumPowerup()
    {
        // Collect 3 target / goal items


        // Grab the items
        Item[] items = LevelManager.Instance.Items();
        // Grab the goal items
        ItemLevelData[] goals = GoalManager.Instance.Goals;
        // Grab the goal that has greatest amount
        ItemLevelData? greatestGoal = GetGreatestGoal(goals);
        // Grab 3 items

        if (greatestGoal == null)
        {
            Debug.LogWarning("No goals found!");
            return;
        }

        ItemLevelData targetGoal = (ItemLevelData)greatestGoal;

        List<Item> itemsToCollect = new List<Item>();

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].Type == targetGoal.itemPrefab.Type)
            {
                itemsToCollect.Add(items[i]);

                if (itemsToCollect.Count >= 3)
                    break;
            }
        }

        for (int i = itemsToCollect.Count - 1; i >= 0; i--)
        {
            itemPickedUp?.Invoke(itemsToCollect[i]);
            Destroy(itemsToCollect[i].gameObject);
        }
    }

    private ItemLevelData? GetGreatestGoal(ItemLevelData[] goals)
    {
        int max = 0;
        int goalIndex = -1;

        for (int i = 0; i < goals.Length; i++)
        {
            if (goals[i].amount >= max)
            {
                max = goals[i].amount;
                goalIndex = i;
            }
        }


        if (goalIndex <= -1)
            return null;
        return goals[goalIndex];
    }
}

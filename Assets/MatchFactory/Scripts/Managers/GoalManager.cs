using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GoalManager : MonoBehaviour
{

    [Header("Data")]
    private ItemLevelData[] goals;
    private List<GoalCard> goalCards = new List<GoalCard>();

    [Header("Elements")]
    [SerializeField] private Transform goalCardsParent;
    [SerializeField] private GoalCard goalCardPrefab;

    void Awake()
    {
        LevelManager.levelSpawned += OnLevelSpawned;
        ItemSpotManager.itemPickedUp += OnItemPickedUp;
    }



    void OnDestroy()
    {
        LevelManager.levelSpawned -= OnLevelSpawned;
        ItemSpotManager.itemPickedUp -= OnItemPickedUp;
    }

    private void OnLevelSpawned(Level level)
    {
        goals = level.GetGoals();

        GenerateGoalCards();
    }

    private void GenerateGoalCards()
    {
        foreach (var goal in goals)
        {
            GenerateGoalCard(goal);
        }
    }

    private void GenerateGoalCard(ItemLevelData goal)
    {
        GoalCard cardInstance = Instantiate(goalCardPrefab, goalCardsParent);
        cardInstance.Configure(goal.amount, goal.itemPrefab.Icon);

        goalCards.Add(cardInstance);
    }

    private void OnItemPickedUp(Item item)
    {
        for (int i = 0; i < goals.Length; i++)
        {
            if (!goals[i].itemPrefab.Type.Equals(item.Type))
            {
                continue;
            }

            goals[i].amount--;

            if (goals[i].amount <= 0)
            {
                Debug.Log($"Goal completed for item type: {item.Type}");
                CompleteGoal(i);
            }
            else
            {
                Debug.Log($"Goal updated for item type: {item.Type}, remaining amount: {goals[i].amount}");
                goalCards[i].UpdateAmount(goals[i].amount);
            }
        }
    }

    private void CompleteGoal(int goalIndex)
    {
        Debug.Log($"Goal completed for item type: {goals[goalIndex].itemPrefab.Type}");

        goalCards[goalIndex].Complete();

        CheckForLevelComplete();
    }

    private void CheckForLevelComplete()
    {
        foreach (var goal in goals)
        {
            if (goal.amount > 0)
            {
                return;
            }
        }

        Debug.Log("All goals completed! Level complete!");

    }
}

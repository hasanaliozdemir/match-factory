using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{

    [Header("Actions")]
    public static Action<Item> itemPickedUp;
    public static Action<Item> itemBackToGame;

    [Header("Settings")]
    private bool isBusy = false;
    private int vacuumItemsToCollect;
    private int vacuumCounter;

    [Header("Vacuum Elements")]
    [SerializeField] private Transform vacuumSuckPosition;
    [SerializeField] private Vacuum vacuum;

    [Header("Fan Settings")]
    [SerializeField] private float fanMagnitude;

    [Header("FreezeGun Settings")]
    [SerializeField] private float freezeGunDuration;

    [Header("Data")]
    [SerializeField] private int initialPUCount;
    private int vacuumPUCount;

    void Awake()
    {
        LoadData();

        Vacuum.started += OnVacuumStarted;
        InputManager.powerupClicked += OnPowerupClicked;
    }

    void OnDestroy()
    {
        Vacuum.started -= OnVacuumStarted;
        InputManager.powerupClicked -= OnPowerupClicked;
    }

    private void OnPowerupClicked(PowerUp powerup)
    {
        if (isBusy)
            return;

        switch (powerup.PowerUpType)
        {
            case EPowerUpType.Vacuum:
                HandleVacuumClicked();
                break;
            case EPowerUpType.Spring:
                // Handle Spring powerup
                break;
            case EPowerUpType.Fan:
                // Handle Fan powerup
                break;
            case EPowerUpType.FreezeGun:
                // Handle FreezeGun powerup
                break;
            default:
                Debug.LogWarning("Unhandled powerup type: " + powerup.PowerUpType);
                break;
        }
    }

    private void HandleVacuumClicked()
    {
        if (vacuumPUCount <= 0)
        {
            vacuumPUCount = 3;
            SaveData();
        }
        else
        {
            isBusy = true;

            vacuumPUCount--;
            SaveData();

            vacuum.Play();
        }
    }

    private void OnVacuumStarted()
    {
        VacuumPowerup();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    #region Vacuum
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

        vacuumCounter = 0;

        ItemLevelData targetGoal = (ItemLevelData)greatestGoal;

        List<Item> itemsToCollect = new List<Item>();

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                continue;
            }
            if (items[i].Type == targetGoal.itemPrefab.Type)
            {
                itemsToCollect.Add(items[i]);

                if (itemsToCollect.Count >= 3)
                    break;
            }
        }

        vacuumItemsToCollect = itemsToCollect.Count;

        for (int i = 0; i < itemsToCollect.Count; i++)
        {
            itemsToCollect[i].DisablePhysics();

            Item itemToCollect = itemsToCollect[i];

            List<Vector3> points = new List<Vector3>();


            points.Add(itemsToCollect[i].transform.position);
            points.Add(itemsToCollect[i].transform.position);
            points.Add(itemsToCollect[i].transform.position + Vector3.up * 2);
            points.Add(vacuumSuckPosition.position + Vector3.up * 2);
            points.Add(vacuumSuckPosition.position);
            points.Add(vacuumSuckPosition.position);

            LeanTween.moveSpline(itemsToCollect[i].gameObject, points.ToArray(), 0.75f).setEase(LeanTweenType.easeInCubic).setOnComplete(() => ItemReachedVacuum(itemToCollect));

            LeanTween.scale(itemsToCollect[i].gameObject,
                Vector3.zero, 0.75f).setEase(LeanTweenType.easeInCubic);
        }

        for (int i = 0; i < itemsToCollect.Count; i++)
        {
            itemPickedUp?.Invoke(itemsToCollect[i]);
        }

    }

    private void ItemReachedVacuum(Item item)
    {
        vacuumCounter++;

        if (vacuumCounter >= vacuumItemsToCollect)
        {
            isBusy = false;
        }

        Destroy(item.gameObject);
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

    private void UpdateVacuumVisuals()
    {
        vacuum.UpdateVisuals(vacuumPUCount);
    }
    #endregion


    #region Spring

    [Button]
    public void SpringPowerUp()
    {


        ItemSpot spot = ItemSpotManager.instance.GetRandomOccupiedSpot();

        if (spot == null)
        {
            return;
        }

        isBusy = true;

        Item itemToRelease = spot.Item;

        spot.Clear();

        itemToRelease.UnassignSpot();
        itemToRelease.EnablePhysics();
        itemToRelease.EnableShadow();

        itemToRelease.transform.parent = LevelManager.Instance.ItemParent;
        itemToRelease.transform.localPosition = Vector3.up * 3;
        itemToRelease.transform.localScale = Vector3.one;

        itemBackToGame?.Invoke(itemToRelease);
    }




    #endregion

    #region Fan

    [Button]
    public void FanPowerUp()
    {

        Item[] items = LevelManager.Instance.Items();

        foreach (Item item in items)
        {
            Debug.Log("Applying fan force to item");
            item.ApplyRandomForce(fanMagnitude);
        }

    }

    #endregion

    #region FreezeGun

    [Button]
    public void FreezeGunPowerUp()
    {
        // Freeze Level timer for 30secs
        TimerManager.Instance.FreezeTimer(freezeGunDuration);
    }


    #endregion
    private void LoadData()
    {
        // TODO: bug var burda bir yerde ama uğraşamıcam
        vacuumPUCount = PlayerPrefs.GetInt("VacuumCount", initialPUCount);
        Debug.Log("VacumCount : ${vacuumPUCount}");
        UpdateVacuumVisuals();
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt("VacuumCount", vacuumPUCount);
        UpdateVacuumVisuals();
    }
}

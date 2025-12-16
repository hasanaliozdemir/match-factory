using System;
using UnityEngine;

public class LevelManager : MonoBehaviour, IGameStateListener
{
    [Header("Data")]
    [SerializeField] private Level[] levels;

    private const string LEVEL_KEY = "Level";
    private int levelIndex;

    [Header("Settings")]
    Level currentLevel;

    [Header("Action")]
    public static Action<Level> levelSpawned;

    void Awake()
    {
        LoadData();
    }

    void Start()
    {

    }

    private void SpawnLevel()
    {

        transform.Clear(); // Extension method to clear children

        int validatedLevelIndex = levelIndex % levels.Length; // Incase we run out of levels

        currentLevel = Instantiate(levels[validatedLevelIndex], transform);

        levelSpawned?.Invoke(currentLevel);
    }

    private void LoadData()
    {
        levelIndex = PlayerPrefs.GetInt(LEVEL_KEY);
        foreach (Level level in levels)
        {
            ItemLevelData[] goals = level.GetGoals();
            Debug.Log($"Level {level.name} has {goals.Length} goals.");
        }
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt(LEVEL_KEY, levelIndex);
    }

    public void GameStateChangedCallback(GameStateEnum newState)
    {
        if (newState == GameStateEnum.GAME)
            SpawnLevel();
        else if (newState == GameStateEnum.LEVELCOMPLETE)
        {
            levelIndex++;
            SaveData();
        }
    }
}

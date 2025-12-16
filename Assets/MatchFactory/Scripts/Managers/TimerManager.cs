using System;
using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour, IGameStateListener
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI timerText;
    private int currentTimer;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Awake()
    {
        LevelManager.levelSpawned += OnLevelSpawned;
    }
    void OnDestroy()
    {
        LevelManager.levelSpawned -= OnLevelSpawned;
    }

    private void OnLevelSpawned(Level level)
    {
        currentTimer = level.Duration;
        timerText.text = TimeSpan.FromSeconds(level.Duration).ToString(@"mm\:ss");

        StartTimer();
    }

    private void StartTimer()
    {
        InvokeRepeating(nameof(UpdateTimer), 1f, 1f);
    }

    private void UpdateTimer()
    {
        currentTimer--;
        timerText.text = TimeSpan.FromSeconds(currentTimer).ToString(@"mm\:ss");

        if (currentTimer <= 0)
        {
            TimerFinished();
        }
    }

    private void TimerFinished()
    {
        CancelInvoke(nameof(UpdateTimer));
        GameManager.instance.SetGameState(GameStateEnum.GAMEOVER);
    }

    public void GameStateChangedCallback(GameStateEnum newState)
    {
        if (newState != GameStateEnum.GAME)
        {
            CancelInvoke(nameof(UpdateTimer));
        }
    }
}

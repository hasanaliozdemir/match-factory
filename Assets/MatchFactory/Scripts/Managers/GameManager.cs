using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    private GameStateEnum gameState;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SetGameState(GameStateEnum.MENU);
    }

    public void SetGameState(GameStateEnum newState)
    {
        this.gameState = newState;
        // Notify listeners about the state change
        // (Implementation of notification logic goes here)
        IEnumerable<IGameStateListener> gameStateListeners
            = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<IGameStateListener>();

        foreach (IGameStateListener dependency in gameStateListeners)
        {
            dependency.GameStateChangedCallback(newState);
        }
    }

    public void StartGame()
    {
        SetGameState(GameStateEnum.GAME);
    }

    public bool IsGame
    {
        get { return gameState == GameStateEnum.GAME; }
    }

    public void NextButtonCallback()
    {
        SceneManager.LoadScene(0);
        SetGameState(GameStateEnum.GAME);
    }

    public void RetryButtonCallback()
    {
        SceneManager.LoadScene(0);
        // For simplicity, just restart the game
        SetGameState(GameStateEnum.GAME);
    }
}

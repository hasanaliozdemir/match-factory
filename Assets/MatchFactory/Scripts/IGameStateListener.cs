using UnityEngine;

public interface IGameStateListener
{
    void GameStateChangedCallback(GameStateEnum newState);
}

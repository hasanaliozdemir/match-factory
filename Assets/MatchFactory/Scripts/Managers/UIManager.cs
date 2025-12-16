using UnityEngine;

public class UIManager : MonoBehaviour, IGameStateListener
{
    [Header("Panels")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject levelCompletePanel;
    [SerializeField] private GameObject gameOverPanel;

    public void GameStateChangedCallback(GameStateEnum newState)
    {
        menuPanel.SetActive(newState == GameStateEnum.MENU);
        gamePanel.SetActive(newState == GameStateEnum.GAME);
        levelCompletePanel.SetActive(newState == GameStateEnum.LEVELCOMPLETE);
        gameOverPanel.SetActive(newState == GameStateEnum.GAMEOVER);
    }

}

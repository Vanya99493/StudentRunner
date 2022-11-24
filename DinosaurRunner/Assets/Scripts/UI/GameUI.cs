using UnityEngine;

public class GameUI : MonoBehaviour
{
    public event System.Action OnStartGame;
    public event System.Action<string> OnChangePlayerName;
    public event System.Action OnPauseGame;
    public event System.Action OnUnpauseGame;
    public event System.Action<int> OnEndGame;
    public event System.Action OnRestartGame;
    public event System.Action OnToMainMenu;
    public event System.Action OnExitGame;
    public event System.Action OnPressButton;
    
    // Mobile events
    public event System.Action OnPressButtonPlayerJump;
    public event System.Action OnPressButtonPlayerCrouch;

    [Header("Panels")]
    [SerializeField] private MainMenuPanel _mainMenuPanel;
    [SerializeField] private ChangePlayerNamePanel _changePlayerNamePanel;
    [SerializeField] private RecordsPanel _recordsPanel;
    [SerializeField] private GameHoodPanel _gameHoodPanel;
    [SerializeField] private GamePausePanel _gamePausePanel;
    [SerializeField] private EndGamePanel _endGamePanel;

    public void Initialize(bool isMobile)
    {
        _gameHoodPanel.ControlPlatform(isMobile);
        MoveToMainMenu();
    }

    public void UpdateScore(int score)
    {
        _gameHoodPanel.UpdateScore(score);
    }

    public void UpdateRecords(string _path)
    {
        _recordsPanel.UpdateRecordsView(_path);
    }

    public void EndGameLevel()
    {
        HideAllPanels();
        _endGamePanel.gameObject.SetActive(true);
    }

    public void UpdatePlayerName(string newPlayerName)
    {
        _mainMenuPanel.ChangePlayerName(newPlayerName);
    }

    #region Mobile methods
    public void PressButtonPlayerJump()
    {
        OnPressButtonPlayerJump?.Invoke();
    }
    public void PressButtonPlayerCrouch()
    {
        OnPressButtonPlayerCrouch?.Invoke();
    }
    #endregion

    #region Buttons methods
    public void BackToMainMenu()
    {
        OnPressButton?.Invoke();
        HideAllPanels();
        _mainMenuPanel.gameObject.SetActive(true);
    }

    public void MoveToMainMenu()
    {
        HideAllPanels();
        _mainMenuPanel.gameObject.SetActive(true);
        OnRestartGame?.Invoke();
        OnToMainMenu?.Invoke();
    }

    public void StartGame()
    {
        OnPressButton?.Invoke();
        HideAllPanels();
        _gameHoodPanel.gameObject.SetActive(true);
        OnStartGame?.Invoke();
    }

    public void OpenRecords()
    {
        OnPressButton?.Invoke();
        HideAllPanels();
        _recordsPanel.gameObject.SetActive(true);
    }

    public void ExitGame()
    {
        OnPressButton?.Invoke();
        OnExitGame?.Invoke();
    }

    public void OpenChangePlayerNamePanel()
    {
        HideAllPanels();
        _changePlayerNamePanel.gameObject.SetActive(true);
    }

    public void AgreeChangePlayerName()
    {
        OnChangePlayerName?.Invoke(_changePlayerNamePanel.PlayerName);
        BackToMainMenu();
    }

    public void CancelChangePlayerName()
    {
        BackToMainMenu();
    }

    public void PauseGame()
    {
        OnPressButton?.Invoke();
        HideAllPanels();
        _gamePausePanel.gameObject.SetActive(true);
        OnPauseGame?.Invoke();
    }

    public void UnpauseGame()
    {
        OnPressButton?.Invoke();
        HideAllPanels();
        _gameHoodPanel.gameObject.SetActive(true);
        OnUnpauseGame?.Invoke();
    }

    public void StopGame()
    {
        OnPressButton?.Invoke();
        OnEndGame?.Invoke(_gameHoodPanel.GetScore());
        MoveToMainMenu();
    }

    public void RestartGame()
    {
        OnEndGame?.Invoke(_gameHoodPanel.GetScore());
        OnRestartGame?.Invoke();
        StartGame();
    }
    #endregion

    private void HideAllPanels()
    {
        _mainMenuPanel.gameObject.SetActive(false);
        _changePlayerNamePanel.gameObject.SetActive(false);
        _recordsPanel.gameObject.SetActive(false);
        _gameHoodPanel.gameObject.SetActive(false);
        _gamePausePanel.gameObject.SetActive(false);
        _endGamePanel.gameObject.SetActive(false);
    }
}
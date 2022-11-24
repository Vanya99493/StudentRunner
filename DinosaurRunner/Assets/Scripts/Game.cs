using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{
    [Header("Game components links")]
    [SerializeField] private TilesMover _tilesMover;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private EnemiesManager _enemiesManager;
    [SerializeField] private GameUI _gameUI;
    [SerializeField] private AudioManager _audioManager;

    [Header("Player settings")]
    [SerializeField] private float _playerSpeed;
    [SerializeField] private float _playerJumpForce;
    [SerializeField] private float _crouchingTime;

    [Header("Level settings")]
    [SerializeField] private float _complexityCoefficient;
    [SerializeField] private float _delayBeforeComplexityIncrease;

    [Header("Spawn enemies settings")]
    [SerializeField] private float _startDelay;
    [SerializeField] private float _spawnDelay;

    [Header("Enemies pool settings")]
    [SerializeField] private int _numberOfPedestrianEnemiesPool;
    [SerializeField] private int _numberOFlyingEnemiesPool;

    [Header("Enemies hide settings")]
    [SerializeField] private float _xPositionToHideEnemy;

    [Header("Tiles move settings")]
    [SerializeField] private float _xPositionToRepeatTiles;
    [SerializeField] private bool _directionToRight;

    [Header("Score settings")]
    [SerializeField] private float _updateScoreCoefficient;
    [SerializeField] private float _delayBeforeScoreUpdate;

    [Header("Audio settings")]
    [SerializeField] private float _musicVolume;
    [SerializeField] private float _eventVolume;

    private ComplexityIncreaser _complexityIncreaser;
    private ScoreUpdater _scoreUpdater;
    private RecordsConverter _recordsConverter;
    private bool _isMobile;
    private bool _isStartGame;
    private float _speed;
    private string _playerName;

    private Coroutine _spawnEnemiesCoroutine;
    private Coroutine _updateSpeedCoroutine;
    private Coroutine _updateScoreCoroutine;

    private void Awake()
    {
        _isMobile = Application.isMobilePlatform;
        _isStartGame = false;

        _tilesMover.Initialize(_xPositionToRepeatTiles, _directionToRight);
        _playerController.Initialize(_playerJumpForce, _crouchingTime);
        _enemiesManager.Initialize(_numberOfPedestrianEnemiesPool, _numberOFlyingEnemiesPool, _xPositionToHideEnemy);
        _gameUI.Initialize(_isMobile);
        _audioManager.Initialize(_musicVolume, _eventVolume);

        _complexityIncreaser = new ComplexityIncreaser(_complexityCoefficient);
        _scoreUpdater = new ScoreUpdater(_updateScoreCoefficient);
        _recordsConverter = new RecordsConverter();

        _gameUI.UpdateRecords(_recordsConverter.PathToJson);

        string playerName = PlayerPrefs.GetString("PlayerName");
        ChangePlayerName(playerName == "" ? "Player" : playerName);
    }

    private void Start()
    {
        _gameUI.OnPressButtonPlayerJump += PlayerJump;
        _gameUI.OnPressButtonPlayerCrouch += PlayerCrouch;

        _gameUI.OnStartGame += StartGame;
        _gameUI.OnChangePlayerName += ChangePlayerName;
        _gameUI.OnPauseGame += PauseGame;
        _gameUI.OnUnpauseGame += UnpauseGame;
        _gameUI.OnEndGame += UpdateRecords;
        _gameUI.OnRestartGame += ResetState;
        _gameUI.OnToMainMenu += _audioManager.PlayMainMenuAudio;
        _gameUI.OnExitGame += ExitGame;
        _gameUI.OnPressButton += _audioManager.PlayPressButtonAudio;
        _playerController.OnDie += EndGame;
        _playerController.OnDie += _audioManager.PlayDiePlayerAudio;
        _playerController.OnDie += () => _recordsConverter.AddRecordItem(_playerName, _scoreUpdater.GetIntegerScore());
        _playerController.OnDie += () => _gameUI.UpdateRecords(_recordsConverter.PathToJson);

        _audioManager.PlayMainMenuAudio();
    }

    private void Update()
    {
        if (!_isStartGame)
        {
            return;
        }

        _tilesMover.Move(_speed, Time.deltaTime);
        _enemiesManager.MoveEnemies(_speed, Time.deltaTime);

        if (!_isMobile)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                PlayerJump();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                PlayerCrouch();
            }
        }
    }

    private void PlayerJump()
    {
        _playerController.SetState(PlayerState.Jump);
        _audioManager.PlayJumpAudio();
    }

    private void PlayerCrouch()
    {
        _playerController.SetState(PlayerState.Crouch);
        _audioManager.PlayCrouchAudio();
    }

    private void StartGame()
    {
        UnpauseGame();
        _isStartGame = true;
        _speed = _playerSpeed;

        _playerController.SetState(PlayerState.Run);
        _audioManager.PlayGameAudio();
        _updateSpeedCoroutine = StartCoroutine(UpdateSpeed());
        _spawnEnemiesCoroutine = StartCoroutine(SpawnEnemies());
        _updateScoreCoroutine = StartCoroutine(UpdateScore());
    }

    private void ChangePlayerName(string newPlayerName)
    {
        if(newPlayerName.Trim() == "")
        {
            return;
        }
        _playerName = newPlayerName;
        _gameUI.UpdatePlayerName(_playerName);
        PlayerPrefs.SetString("PlayerName", _playerName);
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
    }

    private void UnpauseGame()
    {
        Time.timeScale = 1;
    }

    private void UpdateRecords(int score)
    {
        EndGame();
    }

    private void EndGame()
    {
        StopCoroutine(_spawnEnemiesCoroutine);
        StopCoroutine(_updateSpeedCoroutine);
        StopCoroutine(_updateScoreCoroutine);
        _enemiesManager.HideAllEnemies();
        _speed = 0;
        _gameUI.EndGameLevel();
    }

    private void ResetState()
    {
        UnpauseGame();
        _playerController.SetState(_playerController.PlayerState == PlayerState.Die ? PlayerState.WakeUp : PlayerState.Idle);
        _scoreUpdater.ResetScore();
        _gameUI.UpdateScore(_scoreUpdater.GetIntegerScore());
    }

    private void ExitGame()
    {
        Application.Quit();
    }

    private IEnumerator UpdateSpeed()
    {
        while (true)
        {
            yield return new WaitForSeconds(_delayBeforeComplexityIncrease);
            _speed *= _complexityIncreaser.UpdateComplexity();
        }
    }

    private IEnumerator SpawnEnemies()
    {
        int numberOfPrefabs = _numberOfPedestrianEnemiesPool + _numberOFlyingEnemiesPool,
            numberOfFlyingPrefabs = _numberOFlyingEnemiesPool;
        
        yield return new WaitForSeconds(_startDelay);

        while (true)
        {
            _enemiesManager.InstantiatePedestrianEnemy().Initialize(_directionToRight);

            yield return new WaitForSeconds(_spawnDelay / 2f);
            if(Random.Range(0, numberOfPrefabs) < numberOfFlyingPrefabs)
            {
                _enemiesManager.InstantiateFlyingEnemy().Initialize(_directionToRight);
            }
            yield return new WaitForSeconds(_spawnDelay / 2f);
        }
    }

    private IEnumerator UpdateScore()
    {
        while (true)
        {
            yield return new WaitForSeconds(_delayBeforeScoreUpdate);
            _scoreUpdater.UpdateScore();
            _gameUI.UpdateScore(_scoreUpdater.GetIntegerScore());
        }
    }
}
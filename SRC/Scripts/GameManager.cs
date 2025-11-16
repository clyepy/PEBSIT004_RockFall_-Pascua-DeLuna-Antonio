using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject _shipPrefab;
    [SerializeField] private GameObject _spaceStationPrefab;
    [SerializeField] private Transform _shipStartPosition;
    [SerializeField] private Transform _spaceStationStartPosition;

    [SerializeField] private SmoothFollow _cameraFollow;
    [SerializeField] private AsteroidSpawner _asteroidSpawner;

    [SerializeField] private GameObject _inGameUI;
    [SerializeField] private GameObject _pausedUI;
    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private GameObject _gameWinUI;
    [SerializeField] private GameObject _mainMenuUI;

    [SerializeField] public Boundary boundary;
    [SerializeField] public GameObject warningUI;

    public GameObject currentShip { get; private set; }
    public GameObject currentSpaceStation { get; private set; }

    public bool gameIsPlaying { get; private set; }
    public bool isPaused;

    void Start()
    {
        // Show the main menu at startup
        ShowMainMenu();
    }

    // Fixed ShowUI: hide all UIs first, then enable the requested UI once
    void ShowUI(GameObject newUI)
    {
    GameObject[] allUI = { _inGameUI, _pausedUI, _gameOverUI, _gameWinUI, _mainMenuUI };


        foreach (GameObject UIToHide in allUI)
        {
            if (UIToHide != null)
                UIToHide.SetActive(false);
        }

        if (newUI != null)
            newUI.SetActive(true);
    }

    public void ShowMainMenu()
    {
        ShowUI(_mainMenuUI);
        gameIsPlaying = false;
        if (_asteroidSpawner != null)
            _asteroidSpawner.isSpawningAsteroids = false;
    }

public void StartGame()
{
    Time.timeScale = 1f; // unpause when starting new game
    Debug.Log("StartGame() called");

    ShowUI(_inGameUI);
    gameIsPlaying = true;

    if (currentShip != null)
        Destroy(currentShip);

    if (currentSpaceStation != null)
        Destroy(currentSpaceStation);

    if (_shipPrefab != null && _shipStartPosition != null)
        currentShip = Instantiate(_shipPrefab, _shipStartPosition.position, _shipStartPosition.rotation);

    if (_spaceStationPrefab != null && _spaceStationStartPosition != null)
        currentSpaceStation = Instantiate(_spaceStationPrefab, _spaceStationStartPosition.position, _spaceStationStartPosition.rotation);

    if (_cameraFollow != null && currentShip != null)
        _cameraFollow.target = currentShip.transform;

    if (_asteroidSpawner != null)
    {
        _asteroidSpawner.isSpawningAsteroids = true;
        if (currentSpaceStation != null)
            _asteroidSpawner.target = currentSpaceStation.transform;
    }

    if (warningUI != null)
        warningUI.SetActive(false);
    
    GameTimer timer = FindObjectOfType<GameTimer>();
    if (timer != null)
    {
        timer.StartTimer();
    }

}


    public void GameOver()
    {
        Debug.Log("GameOver() called");
        ShowUI(_gameOverUI);

        gameIsPlaying = false;

        if (currentShip != null)
        {
            Destroy(currentShip);
        }

        if (currentSpaceStation != null)
        {
            Destroy(currentSpaceStation);
        }

        // Stop showing the warning UI, if it was visible
        if (warningUI != null)
            warningUI.SetActive(false);

        if (_asteroidSpawner != null)
        {
            _asteroidSpawner.isSpawningAsteroids = false;
            _asteroidSpawner.DestroyAllAsteroids();
        }
        GameTimer timer = FindObjectOfType<GameTimer>();
if (timer != null)
    timer.StopTimer();

    }

    public void SetPaused(bool paused)
    {
        if (_inGameUI != null) _inGameUI.SetActive(!paused);
        if (_pausedUI != null) _pausedUI.SetActive(paused);

        if (paused)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }

public void GameWin()
{
    Debug.Log("GameWin() called");
    ShowUI(_gameWinUI);

    gameIsPlaying = false;

    if (currentShip != null)
        Destroy(currentShip);

    if (currentSpaceStation != null)
        Destroy(currentSpaceStation);

    if (_asteroidSpawner != null)
    {
        _asteroidSpawner.isSpawningAsteroids = false;
        _asteroidSpawner.DestroyAllAsteroids();
    }

    if (warningUI != null)
        warningUI.SetActive(false);

    Time.timeScale = 0f; // pause the game when win

    GameTimer timer = FindObjectOfType<GameTimer>();
    if (timer != null)
    timer.StopTimer();

}




    void Update()
    {
        // Boundary checks every frame
        if (currentShip == null || boundary == null || warningUI == null)
            return;

        float distance = (currentShip.transform.position - boundary.transform.position).magnitude;

        if (distance > boundary.destroyRadius)
        {
            Debug.Log("Ship outside destroy radius: " + distance);
            GameOver();
        }
        else if (distance > boundary.warningRadius)
        {
            // Show warning (when outside warning radius, inside destroy radius)
            if (!warningUI.activeSelf)
                Debug.Log("Ship outside warning radius: " + distance);

            warningUI.SetActive(true);
        }
        else
        {
            if (warningUI.activeSelf)
                Debug.Log("Ship back inside safe zone: " + distance);

            warningUI.SetActive(false);
        }
    }
}

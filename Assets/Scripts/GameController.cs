using UnityEngine;
using UnityEngine.UI; // Пространство имен для работы с UI, включая Slider
using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject ballPrefab; 
    public Transform shootPoint; 
    public TextMeshProUGUI[] timerText; 
    public GameObject gameMessage;
    public GameObject winMessage;
    public GameObject loseMessage;
    [SerializeField] private AudioSource audioComponentDestroy;
    public Slider timerSlider; // Поле для слайдера
    private float currentTime; 
    private bool levelCompleted = false;
    private BlockSpawner blockSpawner; 
    private int totalBlocks; 
    private int destroyedBlocks = 0; 
    private int points; // Поле для поинтов
    private Ball _ball;
    private PaddleController paddleController;
    private bool ballLaunched = false;

    void Start()
    {
        points = LoadPoints(); // Загружаем поинты при запуске
        Time.timeScale = 1f;
        blockSpawner = FindObjectOfType<BlockSpawner>();
        paddleController = FindObjectOfType<PaddleController>();

        if (blockSpawner != null)
        {
            totalBlocks = blockSpawner.numberOfBlocks;
        }
        else
        {
            Debug.LogError("BlockSpawner не найден на сцене!");
        }

        SpawnBall();
        winMessage.SetActive(false);
        loseMessage.SetActive(false);
        paddleController.EnableMovement(false);

        GameSettings.Instance.LoadSettings();
        currentTime = TimerManager.Instance.LevelTime; 

        // Настраиваем слайдер
        timerSlider.minValue = 0f; 
        timerSlider.maxValue = currentTime; 
        timerSlider.value = currentTime;
    }

    void Update()
    {
        if (!levelCompleted)
        {
            if (TimerManager.Instance.IsTimerActive)
            {
                currentTime -= Time.deltaTime;
                timerSlider.value = currentTime;
            }
            
            foreach (var textTimer in timerText)
            {
                textTimer.text = $"{Mathf.CeilToInt(currentTime)} sec";
            }

            if (currentTime <= 0)
            {
                GameOver();
            }

            if (destroyedBlocks >= totalBlocks)
            {
                LevelCompleted();
            }

            if (!ballLaunched && Input.GetMouseButtonUp(0))
            {
                LaunchBall();
            }
        }
    }

    public void LaunchBall()
    {
        if (_ball != null)
        {
            _ball.RespawnBall(); 
            ballLaunched = true;  
            paddleController.EnableMovement(true); 
        }
    }

    void SpawnBall()
    {
        GameObject ball = Instantiate(ballPrefab, shootPoint.position, Quaternion.identity, shootPoint);
        _ball = ball.GetComponent<Ball>();  
        ballLaunched = false;               
        paddleController.ResetPosition(); 
    }

    public void BlockDestroyed()
    {
        audioComponentDestroy.Play();
        VibrationManager.Instance.TriggerVibration();
        destroyedBlocks++;
    }

    void GameOver()
    {
        points += 10; // Добавляем 10 поинтов за проигрыш
        SavePoints(points); // Сохраняем поинты
        VibrationManager.Instance.TriggerVibration();
        gameMessage.SetActive(false);
        levelCompleted = true;
        loseMessage.SetActive(true);
        Debug.Log("Game Over! Time's up.");
        Time.timeScale = 0;
    }

    void LevelCompleted()
    {
        points += 100; // Добавляем 100 поинтов за победу
        SavePoints(points); // Сохраняем поинты
        VibrationManager.Instance.TriggerVibration();
        gameMessage.SetActive(false);
        levelCompleted = true;
        winMessage.SetActive(true);
        Debug.Log("Level Completed!");
        Time.timeScale = 0;
    }

    int LoadPoints()
    {
        return PlayerPrefs.GetInt("Points", 0); // Загружаем поинты из PlayerPrefs
    }

    void SavePoints(int value)
    {
        PlayerPrefs.SetInt("Points", value); // Сохраняем поинты в PlayerPrefs
        PlayerPrefs.Save();
    }
}

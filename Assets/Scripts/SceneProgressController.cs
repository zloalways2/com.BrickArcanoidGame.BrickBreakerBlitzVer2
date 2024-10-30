using TMPro;
using UnityEngine;

public class SceneProgressController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelIndicators;
    private int activeLevel;

    private void Awake()
    {
        LoadCurrentLevel();
        UpdateLevelText();
    }

    private void LoadCurrentLevel()
    {
        activeLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
        
    }

    private void UpdateLevelText()
    {
        if (activeLevel > 10)
        {
            levelIndicators.text = "Level  " + 10;
        }
        else
        {
            levelIndicators.text = "Level  " + activeLevel;
        }
        
    }
    

    public void WinGame()
    {
        PlayerPrefs.SetInt("CurrentLevel", activeLevel + 1);
        PlayerPrefs.Save();

        var levelTracker = FindObjectOfType<LevelSystemController>();
        levelTracker.MarkLevelAsCompleted(activeLevel);
    }
}
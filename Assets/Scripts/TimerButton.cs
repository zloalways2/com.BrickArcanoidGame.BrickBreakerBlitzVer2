using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerButton : MonoBehaviour
{
    [SerializeField] private Button timerToggleButton;
    [SerializeField] private Button[] multiplierButtons;
    [SerializeField] private Image timerToggleImage;
    [SerializeField] private Sprite timerOnSprite;
    [SerializeField] private Sprite timerOffSprite;
    [SerializeField] private TextMeshProUGUI _pointsTextBonus;
    [SerializeField] private Button buyBonusButton; // Кнопка для покупки бонусов

    private int points;

    private void Start()
    {
        buyBonusButton.onClick.AddListener(PurchaseBonus);
        points = PlayerPrefs.GetInt("Points", 0);
        _pointsTextBonus.text = $"{points}p";
        timerToggleButton.onClick.AddListener(ToggleTimer);

        // Загружаем состояние бонусных кнопок
        LoadBonusButtonsState();

        GameSettings.Instance.LoadSettings();
        UpdateButtonSprites();
        UpdateMultiplierButtonState();
        foreach (var button in multiplierButtons)
        {
            button.onClick.AddListener(() => SetLevelTimeMultiplier(int.Parse(button.name))); // Используйте имя кнопки как множитель
        }
    }
    public void SetLevelTimeMultiplier(int multiplier)
    {
        GameSettings.Instance.SetTimeMultiplier(multiplier); // Устанавливаем новое время уровня с множителем
        UpdateMultiplierButtonSprites(); // Обновляем спрайты кнопок множителей
    }
    void ToggleTimer()
    {
        GameSettings.Instance.ToggleTimer();
        UpdateButtonSprites();
    }

    public void PurchaseBonus()
    {
        int cost = 1000;

        // Находим первую неактивную кнопку множителя
        for (int i = 0; i < multiplierButtons.Length; i++)
        {
            if (points >= cost && !multiplierButtons[i].interactable)
            {
                points -= cost;
                SavePoints(points);
                multiplierButtons[i].interactable = true; // Разблокируем кнопку множителя
                _pointsTextBonus.text = $"{points}p";
                SaveBonusButtonState(i); // Сохраняем состояние кнопки
                UpdateMultiplierButtonState();
                break; // Разблокируем только одну кнопку за раз
            }
        }
    }

    void UpdateButtonSprites()
    {
        timerToggleImage.sprite = GameSettings.Instance.IsTimerActive ? timerOnSprite : timerOffSprite;
    }

    void UpdateMultiplierButtonState()
    {
        _pointsTextBonus.text = $"{points}p";
        buyBonusButton.interactable = points >= 1000;
    }

    void SavePoints(int value)
    {
        PlayerPrefs.SetInt("Points", value);
        PlayerPrefs.Save();
    }

    void SaveBonusButtonState(int index)
    {
        PlayerPrefs.SetInt($"BonusButton_{index}", 1); // Сохраняем состояние кнопки как активной
        PlayerPrefs.Save();
    }
    private void UpdateMultiplierButtonSprites()
    {
        foreach (var button in multiplierButtons)
        {
            int multiplier = int.Parse(button.name); // Получаем множитель из имени кнопки
            Image buttonImage = button.GetComponent<Image>(); // Получаем компонент Image кнопки

            if (multiplier == GameSettings.Instance.TimeMultiplier)
            {
                buttonImage.sprite = timerOnSprite; // Устанавливаем спрайт для выбранного множителя
            }
            else
            {
                buttonImage.sprite = timerOffSprite; // Устанавливаем спрайт для невыбранного множителя
            }
        }
    }
    void LoadBonusButtonsState()
    {
        // Загружаем состояние каждой бонусной кнопки
        for (int i = 0; i < multiplierButtons.Length; i++)
        {
            if (PlayerPrefs.GetInt($"BonusButton_{i}", 0) == 1)
            {
                multiplierButtons[i].interactable = true; // Устанавливаем кнопку как активную
            }
            else
            {
                multiplierButtons[i].interactable = false; // Устанавливаем кнопку как неактивную
                multiplierButtons[i].image.sprite = timerOffSprite; // Устанавливаем спрайт для невыбранного множителя
            }
        }
    }
}

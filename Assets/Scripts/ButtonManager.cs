using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public static ButtonManager Instance { get; private set; }
    
    public Button[] multiplierButtons;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Не уничтожать объект при загрузке новой сцены
    }

    public void DisableAllButtons()
    {
        foreach (Button button in multiplierButtons)
        {
            button.interactable = false;
        }
    }

    public void EnableButton(int index)
    {
        if (index >= 0 && index < multiplierButtons.Length)
        {
            multiplierButtons[index].interactable = true;
        }
    }

    public void DisableButton(int index)
    {
        if (index >= 0 && index < multiplierButtons.Length)
        {
            multiplierButtons[index].interactable = false;
        }
    }
}
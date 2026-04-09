using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance { get; private set; }
    public TMP_Text timerText;
    public TMP_Text valueText;
    public TMP_Text healthPercentText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void SetTimerText(string text)
    {
        timerText.text = text;
    }
    
    public void SetValueText(int value)
    {
        valueText.text = value.ToString() + "p";
    }

    public void SetHealthText(int value)
    {
        healthPercentText.text = value.ToString() + "%";
    }

}

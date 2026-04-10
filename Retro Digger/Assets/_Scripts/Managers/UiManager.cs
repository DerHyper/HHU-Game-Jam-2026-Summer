using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public TMP_Text timerText;
    public TMP_Text valueText;
    public TMP_Text healthPercentText;

    void Start()
    {
        TimeManager.Instance.TimeChanged += SetTimerText;
        UiInformer.OnValueChanged += SetValueText;
        UiInformer.OnHealthChanged += SetHealthText;
    }

    void OnDestroy()
    {
        TimeManager.Instance.TimeChanged -= SetTimerText;

        UiInformer.OnValueChanged -= SetValueText;
        UiInformer.OnHealthChanged -= SetHealthText;
    }

    void SetTimerText(string text)
    {
        if (timerText == null) return;
        timerText.text = text;
    }

    void SetValueText(int value)
    {
        if (valueText == null) return;
        valueText.text = value.ToString() + "p";
    }

    void SetHealthText(int value)
    {
        if (healthPercentText == null) return;
        healthPercentText.text = value.ToString() + "%";
    }


    public static class UiInformer
    {
        public static event System.Action<int> OnValueChanged;
        public static event System.Action<int> OnHealthChanged;

        public static void SetValueText(int value) => OnValueChanged?.Invoke(value);
        public static void SetHealthText(int value) => OnHealthChanged?.Invoke(value);
    }
}

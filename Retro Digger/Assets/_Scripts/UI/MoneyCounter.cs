using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyCounter : MonoBehaviour
{
    private TMP_Text _moneyText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _moneyText = GetComponentInChildren<TMP_Text>();

        MoneyManager.Instance.OnMoneyChanged += UpdateMoneyText;
        UpdateMoneyText(MoneyManager.Instance.CurrentMoney);
    }

    void OnDestroy()
    {
        MoneyManager.Instance.OnMoneyChanged -= UpdateMoneyText;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateMoneyText(int money)
    {
        _moneyText.text = $"{money}p";
    }
}

using TMPro;
using UnityEngine;

public class ToolBuyButton : MonoBehaviour
{
    public string toolName;

    public TMP_Text priceText;

    private readonly ToolService _toolService = ToolService.Instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var button = GetComponent<UnityEngine.UI.Button>();
        button.onClick.AddListener(() =>
        {
            _toolService.BuyTool(toolName);
        });

        MoneyManager.Instance.OnMoneyChanged += (_) => UpdateButton();

        UpdateButton();
    }

    void OnDestroy()
    {
        MoneyManager.Instance.OnMoneyChanged -= (_) => UpdateButton();
    }

    void UpdateButton()
    {
        var button = GetComponent<UnityEngine.UI.Button>();
        button.interactable = CanBuyTool();
        UpdatePriceTag();
    }

    bool CanBuyTool() => _toolService.CanBuyTool(toolName);

    void UpdatePriceTag()
    {
        var price = _toolService.GetToolModelToBuy(toolName)?.PointPrice.ToString();
        priceText.text = price is null ? "N/A" : $"{price}p";
    }

    // Update is called once per frame
    void Update()
    {

    }
}

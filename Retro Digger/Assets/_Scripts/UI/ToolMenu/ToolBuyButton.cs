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
        var toolModel = _toolService.GetToolModel(toolName);

        var button = GetComponent<UnityEngine.UI.Button>();

        button.interactable = _toolService.CanBuyTool(toolName);
        button.onClick.AddListener(() =>
        {
            if (_toolService.BuyTool(toolName))
                button.interactable = false;
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}

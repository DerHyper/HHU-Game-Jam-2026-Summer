using UnityEngine;

public class ToolBuyButton : MonoBehaviour
{
    public string toolName;
    public int toolCost;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var button = GetComponent<UnityEngine.UI.Button>();

        button.interactable = MoneyManager.Instance.CurrentMoney >= toolCost;
        button.onClick.AddListener(() =>
        {
            if (MoneyManager.Instance.CurrentMoney >= toolCost)
            {
                MoneyManager.Instance.PayMoney(toolCost);
                var tool = InventoryManager.Instance.CollectedTools.Add(
                    new Tool(1, 10);
                );
                InventoryManager.Instance.AddTool(tool);
            }
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}

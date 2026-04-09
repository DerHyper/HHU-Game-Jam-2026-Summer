using System;
using System.Collections.Generic;

public class ToolService
{
    public static readonly ToolService Instance = new();
    private ToolService() { }

    private readonly List<ToolModels> _availableTools = new()
    {
        ToolModels.Brush,
    };


    public ToolModels GetToolModel(string toolName)
    {
        return toolName switch
        {
            "Hammer" => ToolModels.Hammer,
            "Chisel" => ToolModels.Chisel,
            "Brush" => ToolModels.Brush,
            _ => null
        };
    }

    public bool CanBuyTool(string toolName)
    {
        var toolModel = GetToolModel(toolName);
        return toolModel != null && MoneyManager.Instance.CurrentMoney >= toolModel.PointPrice;
    }

    public bool BuyTool(string toolName)
    {
        var toolModel = GetToolModel(toolName);
        if (toolModel == null || MoneyManager.Instance.CurrentMoney < toolModel.PointPrice)
        {
            return false;
        }

        MoneyManager.Instance.PayMoney(toolModel.PointPrice);
        _availableTools.Add(toolModel);

        return true;
    }

    public bool HasToolWithName(string name) => _availableTools.Exists(t => t.Name == name);
}
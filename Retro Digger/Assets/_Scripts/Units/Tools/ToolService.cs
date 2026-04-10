using System;
using System.Collections.Generic;
using System.Linq;

public class ToolService
{
    public static readonly ToolService Instance = new();
    private ToolService() { }

    private readonly List<ToolModels> _availableTools = new()
    {
        ToolModels.Brush,
    };

    public ToolModels GetToolModelToBuy(string toolName)
    {
        switch (toolName)
        {
            case "Brush" when _availableTools.Count < 3:
            case "Hammer" when _availableTools.Count < 2:
            case "Chisel" when _availableTools.Exists(t => t.Name == "Chisel") && _availableTools.Count < 3:
                return null;
        }

        var toolModel = _availableTools.FirstOrDefault(t => t.Name == toolName);
        toolModel = toolModel == null
            // didn't buy the tool yet or it has the wrong name.
            ? GetToolModel(toolName)
            // bought already a version of the tool
            : toolModel = toolModel.Upgrade();

        return toolModel;
    }

    private ToolModels GetToolModel(string toolName)
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
        var toolModel = GetToolModelToBuy(toolName);
        return toolModel != null && MoneyManager.Instance.CurrentMoney >= toolModel.PointPrice;
    }

    public bool BuyTool(string toolName)
    {
        var toolModel = GetToolModelToBuy(toolName);
        if (toolModel == null || MoneyManager.Instance.CurrentMoney < toolModel.PointPrice)
        {
            return false;
        }

        _availableTools.RemoveAll(t => t.Name == toolName);
        _availableTools.Add(toolModel);
        MoneyManager.Instance.PayMoney(toolModel.PointPrice);

        return true;
    }

    public bool BuyFreeTool(string toolName)
    {
        var toolModel = GetToolModelToBuy(toolName);
        if (toolModel == null)
        {
            return false;
        }

        _availableTools.RemoveAll(t => t.Name == toolName);
        _availableTools.Add(toolModel);

        return true;
    }

    public bool HasToolWithName(string name) => _availableTools.Exists(t => t.Name == name);
    public ToolModels GetCurrentTool(string name) => _availableTools.FirstOrDefault(t => t.Name == name);
}
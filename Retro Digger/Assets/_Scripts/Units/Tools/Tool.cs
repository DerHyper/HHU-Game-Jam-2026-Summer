using UnityEngine;

[System.Serializable]
public class Tool
{
    public string Name;
    public string Description;
    public int Level = 1;
    public int DiggingDamager = 1;
    public Sprite UiIcon;
    public Color UiColor = Color.white;
    public int UiOrder = 0;

    public Tool(int level, int diggingDamager)
    {
        Level = level;
        DiggingDamager = diggingDamager;
    }

    public Tool WithValuesFrom(ToolModels toolModel) 
        => new(Level, toolModel.DiggingDamage)
        {
            Name = Name,
            Description = Description,
            UiIcon = UiIcon,
            UiColor = UiColor,
            UiOrder = UiOrder
        };
}
using UnityEngine;

public sealed record ToolModels
{
    public static readonly ToolModels Hammer = new("Hammer", 3, 1);
    public static readonly ToolModels Chisel = new("Chisel", 2, 1);
    public static readonly ToolModels Brush = new("Brush", 1, 1);

    public string Name { get; private set; }
    public int Level { get; private set; }
    public int DiggingDamage { get; private set; }
    public int PointPrice { get; private set; }

    private ToolModels(string name, int level, int diggingDamage, int pointPrice = 1)
    {
        Name = name;
        Level = level;
        DiggingDamage = diggingDamage;
        PointPrice = pointPrice;
    }


    public ToolModels Upgrade() => this with
    {
        DiggingDamage = DiggingDamage + 1,
        PointPrice = PointPrice + 25,
    };
}

[System.Serializable]
public class Tool
{
    public int Level = 1;
    public int DiggingDamager = 1;

    public Tool(int level, int diggingDamager)
    {
        Level = level;
        DiggingDamager = diggingDamager;
    }
}
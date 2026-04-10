public record GameScene
{
    public static readonly GameScene MainMenu = new("Scenes/MainMenu");
    public static readonly GameScene MapView = new("Scenes/MapViewScene");
    public static readonly GameScene DiggingView = new("Scenes/Digging");
    public static readonly GameScene ToolsShop = new("Scenes/ToolsShopScene");

    public readonly string SceneName;

    private GameScene(string sceneName)
    {
        SceneName = sceneName;
    }
}
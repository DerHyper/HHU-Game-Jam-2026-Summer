public class ScoreEntrySetter : UnityEngine.MonoBehaviour
{
    public TMPro.TMP_Text playerNameText;
    public TMPro.TMP_Text pointsText;

    public void Set(ScoreEntry entry)
    {
        playerNameText.text = entry.playerName;
        pointsText.text = entry.points.ToString() + "p";
    }
}
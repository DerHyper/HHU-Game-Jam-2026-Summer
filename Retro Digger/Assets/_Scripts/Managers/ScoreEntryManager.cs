using TMPro;
using UnityEngine;

public class ScoreEntryManager : MonoBehaviour
{
    public TMP_InputField NameInputField;
    public TMP_Text ScoreText;

    void Start()
    {
        int currentScore = MoneyManager.Instance.CurrentScore;
        ScoreText.text = $"Your Score: {currentScore}";
    }

    public void Submit()
    {
        LeaveScreen(
            string.IsNullOrWhiteSpace(NameInputField.text)
                ? "Anonymous"
                : NameInputField.text, 
            MoneyManager.Instance.CurrentScore);
    }

    public void Skip()
    {
        LeaveScreen("Anonymous", MoneyManager.Instance.CurrentScore);
    }

    private void LeaveScreen(string playerName, int score)
    {
        // Here you would typically save the score to a leaderboard or database
        Debug.Log($"Player Name: {playerName}, Score: {score}");

        // Optionally, you can clear the input field after submission
        NameInputField.text = "";

        SendScore(playerName, score);

        GameManager.Instance.GoToMainMenu();
    }


    private async void SendScore(string playerName, int score)
    {
        try
        {
            await ApiService.Instance.CreateScore(playerName, score);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to send score: {ex.Message}");
        }
    }
}
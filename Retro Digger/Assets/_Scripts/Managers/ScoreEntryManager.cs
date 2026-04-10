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
        string playerName = NameInputField.text;
        int score = MoneyManager.Instance.CurrentScore;

        // Here you would typically save the score to a leaderboard or database
        Debug.Log($"Player Name: {playerName}, Score: {score}");

        // Optionally, you can clear the input field after submission
        NameInputField.text = "";

        SendScore(playerName, score);
    }

    public async void SendScore(string playerName, int score)
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
using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public interface IMoneyManager
{
    /// <summary>
    /// Resets the player's money and score to zero. This method should be called at the start of a new game to ensure that the player starts with a clean slate.
    /// </summary>
    void StartNewGame();

    /// <summary>
    /// Adds the specified amount of money to the player's current money and score. This method should be called when the player earns money when picking up a game.
    /// </summary>
    /// <param name="amount">The amount of money to add</param>
    void AddMoneyAndScore(int amount);

    /// <summary>
    /// Pays the specified amount of money. This method should be called when the player makes a purchase or spends money in the game.
    /// </summary>
    /// <param name="amount">The amount of money to spend</param>
    void PayMoney(int amount);

    /// <summary>
    /// Finishes the game and stores the final score in the score board.
    /// </summary>
    /// <param name="playerName">The name of the player to associate with the score</param>
    Task FinishGameAsync(string playerName);

    event Action<int> OnMoneyChanged;
}

public sealed partial class MoneyManager : IMoneyManager
{
    public int CurrentScore { get; private set; } = 75;

    #region Money Management
    private int _currentMoney = 75;
    public int CurrentMoney
    {
        get => _currentMoney;
        private set
        {
            _currentMoney = value;
            OnMoneyChanged?.Invoke(_currentMoney);
        }
    }
    public event Action<int> OnMoneyChanged;
    #endregion


    public void AddMoneyAndScore(int amount)
    {
        CurrentMoney += amount;
        CurrentScore += amount;
    }

    public void PayMoney(int amount)
    {
        CurrentMoney -= amount;
    }

    public void StartNewGame()
    {
        CurrentMoney = 0;
        CurrentScore = 0;
    }

    public Task FinishGameAsync(string playerName)
        => DataService.GlobalDataService.CreateAsync(
            GUID.Generate().ToString(),
            new Score
            {
                PlayerName = playerName,
                ScoreValue = CurrentScore
            });

    [Serializable]
    private class Score
    {
        public string PlayerName { get; set; }
        public int ScoreValue { get; set; }
    }
}

/// <summary>
/// Singleton class responsible for managing the player's money and score.
/// </summary>
public sealed partial class MoneyManager : MonoBehaviour
{
    #region Singleton Implementation
    public static MoneyManager Instance;
    private MoneyManager() { }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("MoneyManager: Multiple instances detected. Destroying duplicate.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    #endregion

    public DatabaseManager DataService => DatabaseManager.Instance;


    void Start() { }
    void Update() { }
}

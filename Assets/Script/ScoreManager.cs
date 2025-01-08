using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager _instance;

    public static ScoreManager Instance 
    { 
        get
        {
            if (_instance == null)
                Debug.LogError("no ScoreManger instance");

            return _instance;
        } 
    }

    private int score;
    public int Score { get { return score; } }

    private int highestScore;
    public int HighestScore { get { return highestScore; } }

    private void Awake()
    {
        if (_instance != null)
            Destroy(gameObject);
        else _instance = this;

        DontDestroyOnLoad(this);
    }

    private void OnEnable()
    {
        GameManager.GameStateChange += ResetScore;
        GameManager.GameStateChange += UpdateHighestScore;
    }

    private void OnDisable()
    {
        GameManager.GameStateChange -= ResetScore;
        GameManager.GameStateChange -= UpdateHighestScore;
    }

    private void UpdateHighestScore(GameState state)
    {
        if (state != GameState.GameOver)
            return;

        if (score > highestScore)
            highestScore = score;
    }

    public  event Action<int> OnScoreChange;

    public void ScoreChanger()
    {
        score++;
        OnScoreChange?.Invoke(score);
    }

    public event Action<int> SetGameScore;

    private void ResetScore(GameState state)
    {
        if (state != GameState.StartGame)
            return;

        Debug.Log("reset score");

        score = 0;
        SetGameScore?.Invoke(score);

        GameManager.ChangeGameState(GameState.Game);
    }
}

using System;
using UnityEngine;

public static class GameManager
{
    public static event Action<GameState> GameStateChange;

    public static void ChangeGameState(GameState gameState)
    {
        GameStateChange?.Invoke(gameState);
    }
}

public enum GameState
{
    HomeMenu,
    StartGame,
    Game,
    Pause,
    GameOver,
}

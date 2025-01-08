using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject GameUI;
    [SerializeField] private GameObject GameOverUI;

    [SerializeField] private TextMeshProUGUI PlayScoreText;

    [SerializeField] private GameObject NewHigh;
    [SerializeField] private TextMeshProUGUI GameOverScoreText;

    private void OnEnable()
    {
        GameManager.GameStateChange += PauseGame;
        GameManager.GameStateChange += ResumeGame;
        GameManager.GameStateChange += GameOver;

        ScoreManager.Instance.SetGameScore += OnScoreChange;
        ScoreManager.Instance.OnScoreChange += OnScoreChange;
    }

    private void OnDisable()
    {
        GameManager.GameStateChange -= PauseGame;
        GameManager.GameStateChange -= ResumeGame;
        GameManager.GameStateChange -= GameOver;

        ScoreManager.Instance.SetGameScore -= OnScoreChange;
        ScoreManager.Instance.OnScoreChange -= OnScoreChange;
    }

    #region GameState changer
    public void SetGameStatePause()
    {
        GameManager.ChangeGameState(GameState.Pause);
    }

    public void SetGameStateGame()
    {
        GameManager.ChangeGameState(GameState.Game);
    }
    #endregion

    #region enable and disable menu
    private void PauseGame(GameState state)
    {
        if (state != GameState.Pause)
            return;

        Time.timeScale = 0;
        PauseMenu.SetActive(true);
        GameUI.SetActive(false);
    }

    private void ResumeGame(GameState state)
    {
        if (state != GameState.Game)
            return;
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
        GameUI.SetActive(true);
    }

    private void GameOver(GameState state)
    {
        if (state != GameState.GameOver)
            return;

        GameOverUI.SetActive(true);
        GameUI.SetActive(false);

        GameOverScoreText.text = $"score is {ScoreManager.Instance.Score}";

        if (ScoreManager.Instance.Score > ScoreManager.Instance.HighestScore)
            NewHigh.SetActive(true);
        else NewHigh.SetActive(false);
    }
    #endregion

    private void OnScoreChange(int score)
    {
        PlayScoreText.text = score.ToString();
    }

    public void Restart()
    {
        GameManager.ChangeGameState(GameState.StartGame);
        SceneManager.LoadScene(1);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}

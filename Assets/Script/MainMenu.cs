using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.ChangeGameState(GameState.HomeMenu);
        
    }

    public void StartGame()
    {
        GameManager.ChangeGameState(GameState.StartGame);
        SceneManager.LoadScene(1);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonScript : MonoBehaviour
{
    public GameObject LoadingScreen;
    public GameObject MainMenuScreen;
    public GameObject SelectLevelScreen;
    public GameObject ViewScoreScreen;

    public int Level;

    private void Start()
    {
        LoadingScreen.SetActive(false);
    }

    public void StartGame()
    {
        LoadingScreen.SetActive(true);
        GameManager.Lives = GameManager.MaxLives;
        SceneManager.LoadScene("Level_01");
    }

    public void SelectLevel()
    {
        MainMenuScreen.SetActive(false);
        GameManager.Lives = GameManager.MaxLives;
        SelectLevelScreen.SetActive(true);
    }
    public void ViewScore()
    {
        MainMenuScreen.SetActive(false);
        ViewScoreScreen.SetActive(true);
    }

    public void BackToMainMenu()
    {
        SelectLevelScreen.SetActive(false);
        MainMenuScreen.SetActive(true);
        ViewScoreScreen.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void StartLevel()
    {
        LoadingScreen.SetActive(true);
        SceneManager.LoadScene("Level_0" + Level);
    }

    public void NextScoreLevel()
    {
        ScoreManager.Instance.NextLevelScore();
    }

    public void BackScoreLevel()
    {
        ScoreManager.Instance.BackNextLevelScore();
    }
}

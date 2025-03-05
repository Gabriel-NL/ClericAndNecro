using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Pseudo2DMap");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ViewScores()
    {
        SceneManager.LoadScene("HighScoreScene");
    }
}

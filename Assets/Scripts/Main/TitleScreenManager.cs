using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Pseudo2DMap"); // Replace "GameScene" with your actual game scene name
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

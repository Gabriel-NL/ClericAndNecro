using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayScreen : MonoBehaviour
{
    public GameObject howToPlayPanel; // Reference to the panel

    void Start()
    {
        ShowPanel(); // Show the panel at the start
    }

    public void ShowPanel()
    {
        howToPlayPanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }

    public void ClosePanel()
    {
        howToPlayPanel.SetActive(false);
        Time.timeScale = 1f; // Resume the game
    }
}
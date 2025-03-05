using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HighScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI highScoresText;

    void Start()
    {
        LoadScores();
    }

    void LoadScores()
    {
        int numScores = PlayerPrefs.GetInt("NumScores", 0);
        string scoresString = "High Scores:\n";

        for (int i = 0; i < numScores; i++)
        {
            string playerName = PlayerPrefs.GetString("PlayerName" + i, "Unknown");
            int score = PlayerPrefs.GetInt("Score" + i, 0);
            scoresString += (i + 1) + ". " + playerName + " - " + score + "\n";
        }

        highScoresText.text = scoresString;
    }

    public void ReturnToTitle()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    public void ResetScores()
{
    PlayerPrefs.DeleteAll(); // Deletes all stored PlayerPrefs data
    PlayerPrefs.Save();
    LoadScores(); // Refresh the UI
}
}

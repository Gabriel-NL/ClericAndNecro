using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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
        List<(string name, int score)> scoresList = new List<(string, int)>();

        // Retrieve all saved scores
        for (int i = 0; i < numScores; i++)
        {
            string playerName = PlayerPrefs.GetString("PlayerName" + i, "Unknown");
            int score = PlayerPrefs.GetInt("Score" + i, 0);
            scoresList.Add((playerName, score));
        }

        // Sort scores in descending order (highest first)
        scoresList.Sort((a, b) => b.score.CompareTo(a.score));

        // Display the sorted scores
        string scoresString = "High Scores:\n";
        for (int i = 0; i < scoresList.Count; i++)
        {
            scoresString += $"{i + 1}. {scoresList[i].name} - {scoresList[i].score}\n";
        }

        highScoresText.text = scoresString;
    }

    public void ResetScores()
    {
        PlayerPrefs.DeleteAll(); // Clears all saved data
        PlayerPrefs.Save();
        LoadScores(); // Refresh the UI
    }

    public void ReturnToTitle()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public Image gameOverScreen;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI finalScoreText;
    public TMP_InputField nameInputField;
    public Button submitScoreButton;
    public Button returnToTitleButton;
    public Button playAgainButton; // New button

    public float fadeDuration = 2f;
    private int finalScore;

    public AudioSource backgroundMusic; // Reference to background music
    public AudioSource gameOverMusic;    // Reference to gameover theme

    private void Start()
    {
        // Hide all UI elements except the input field initially
        gameOverScreen.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        finalScoreText.gameObject.SetActive(false);
        submitScoreButton.gameObject.SetActive(false);
        returnToTitleButton.gameObject.SetActive(false);
        playAgainButton.gameObject.SetActive(false); // Hide play again button
        nameInputField.gameObject.SetActive(false);
    }

    public void TriggerGameOver()
    {
        finalScore = ScoreManager.Instance.score;
        finalScoreText.text = "Final Score: " + finalScore;

        if (backgroundMusic != null)
        {
            backgroundMusic.Stop();
        }
        if (gameOverMusic != null)
        {
            gameOverMusic.Play();
        }

        StartCoroutine(FadeToBlack());
    }

    private IEnumerator FadeToBlack()
    {
        float elapsedTime = 0f;

        // Show UI elements for fading
        gameOverScreen.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);
        finalScoreText.gameObject.SetActive(true);

        Color screenColor = gameOverScreen.color;
        Color textColor = gameOverText.color;
        Color scoreTextColor = finalScoreText.color;

        while (elapsedTime < fadeDuration)
        {
            float alpha = elapsedTime / fadeDuration;
            gameOverScreen.color = new Color(screenColor.r, screenColor.g, screenColor.b, alpha);
            gameOverText.color = new Color(textColor.r, textColor.g, textColor.b, alpha);
            finalScoreText.color = new Color(scoreTextColor.r, scoreTextColor.g, scoreTextColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Fully show elements after fade
        gameOverScreen.color = new Color(screenColor.r, screenColor.g, screenColor.b, 1);
        gameOverText.color = new Color(textColor.r, textColor.g, textColor.b, 1);
        finalScoreText.color = new Color(scoreTextColor.r, scoreTextColor.g, scoreTextColor.b, 1);

        // Show name input and submit button
        nameInputField.gameObject.SetActive(true);
        submitScoreButton.gameObject.SetActive(true);
    }

    public void SubmitScore()
    {
        string playerName = nameInputField.text.Trim();

        // Limit to 8 characters
        if (playerName.Length > 8)
        {
            playerName = playerName.Substring(0, 8);
        }

        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "Anonymous"; // Default name if empty
        }

        SaveScore(playerName, finalScore);

        // Hide input and show return & play again buttons
        nameInputField.gameObject.SetActive(false);
        submitScoreButton.gameObject.SetActive(false);
        returnToTitleButton.gameObject.SetActive(true);
        playAgainButton.gameObject.SetActive(true); // Show play again button
    }

    private void SaveScore(string name, int score)
    {
        int numScores = PlayerPrefs.GetInt("NumScores", 0);
        PlayerPrefs.SetString("PlayerName" + numScores, name);
        PlayerPrefs.SetInt("Score" + numScores, score);
        PlayerPrefs.SetInt("NumScores", numScores + 1);
        PlayerPrefs.Save();
    }

    public void ReturnToTitle()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    public void PlayAgain() // New function
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reloads the current scene
    }
}

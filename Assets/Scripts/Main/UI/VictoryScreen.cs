using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class VictoryScreen : MonoBehaviour
{
    public GameObject victoryPanel; // Assign the Panel in the Inspector
    public TextMeshProUGUI victoryText;
    public TextMeshProUGUI finalScoreText;
    public TMP_InputField nameInputField;
    public Button submitScoreButton;
    public Button returnToTitleButton;
    public Button playAgainButton; // New button

    public float fadeDuration = 2f;
    private int finalScore;
    private CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = victoryPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = victoryPanel.AddComponent<CanvasGroup>(); // Add CanvasGroup if missing
        }

        // Hide UI elements at the start
        victoryPanel.SetActive(false);
        canvasGroup.alpha = 0;
        nameInputField.gameObject.SetActive(false);
        submitScoreButton.gameObject.SetActive(false);
        returnToTitleButton.gameObject.SetActive(false);
        playAgainButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V)) // Temporary trigger for testing
        {
            TriggerVictory();
        }
    }

    public void TriggerVictory()
    {
        finalScore = ScoreManager.Instance.score;
        finalScoreText.text = "Final Score: " + finalScore;

        victoryPanel.SetActive(true);
        StartCoroutine(FadeInVictoryScreen());
    }

    private IEnumerator FadeInVictoryScreen()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = elapsedTime / fadeDuration;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1;

        // Show input field and submit button after fade-in
        nameInputField.gameObject.SetActive(true);
        submitScoreButton.gameObject.SetActive(true);
    }

    public void SubmitScore()
    {
        string playerName = nameInputField.text.Trim();

        // Limit name to 8 characters
        if (playerName.Length > 8)
        {
            playerName = playerName.Substring(0, 8);
        }

        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "Anonymous"; // Default name if empty
        }

        SaveScore(playerName, finalScore);

        // Hide input field and submit button, show return & play again buttons
        nameInputField.gameObject.SetActive(false);
        submitScoreButton.gameObject.SetActive(false);
        returnToTitleButton.gameObject.SetActive(true);
        playAgainButton.gameObject.SetActive(true);
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

    public void PlayAgain() // Reloads the game
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
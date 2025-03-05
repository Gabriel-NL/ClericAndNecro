using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public Image gameOverScreen; // Assign the black UI image in Inspector
    public TextMeshProUGUI gameOverText; // Assign the red text in Inspector
    public TextMeshProUGUI finalScoreText; // Assign the final score text in Inspector
    public Button returnToTitleButton; // Assign the button in the Inspector
    public float fadeDuration = 2f;

    public void TriggerGameOver()
    {
        finalScoreText.text = "Final Score: " + ScoreManager.Instance.score.ToString();
        finalScoreText.gameObject.SetActive(true);
        returnToTitleButton.gameObject.SetActive(false); // Hide button during fade

        StartCoroutine(FadeToBlack());
    }

    IEnumerator FadeToBlack()
    {
        float elapsedTime = 0f;
        Color screenColor = gameOverScreen.color;
        Color textColor = gameOverText.color;
        Color scoreTextColor = finalScoreText.color;

        gameOverScreen.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);
        finalScoreText.gameObject.SetActive(true);

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            gameOverScreen.color = new Color(screenColor.r, screenColor.g, screenColor.b, alpha);
            gameOverText.color = new Color(textColor.r, textColor.g, textColor.b, alpha);
            finalScoreText.color = new Color(scoreTextColor.r, scoreTextColor.g, scoreTextColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure full visibility after fade
        gameOverScreen.color = new Color(screenColor.r, screenColor.g, screenColor.b, 1);
        gameOverText.color = new Color(textColor.r, textColor.g, textColor.b, 1);
        finalScoreText.color = new Color(scoreTextColor.r, scoreTextColor.g, scoreTextColor.b, 1);

        // Show the button after fade
        returnToTitleButton.gameObject.SetActive(true);
    }

    public void ReturnToTitle()
    {
        SceneManager.LoadScene("TitleScreen"); // Change "TitleScreen" to your actual title screen scene name
    }
}

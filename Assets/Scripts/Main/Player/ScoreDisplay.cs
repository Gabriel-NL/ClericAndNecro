using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    private void Update()
    {
        // Update the text to show the current score from ScoreManager
        scoreText.text = "Score: " + ScoreManager.Instance.score.ToString();
    }
}

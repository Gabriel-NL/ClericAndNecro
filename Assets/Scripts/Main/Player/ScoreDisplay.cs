using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
 /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        Debug.Log("ScoreDisplayInitialized");
    }
    private void FixedUpdate()
    {
        // Update the text to show the current score from ScoreManager
        scoreText.text = "Score: " + ScoreManager.Instance.score.ToString();

    }
}

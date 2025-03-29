using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public int score=0;

    private void Awake()
    {
        
        if (Instance == null || Instance!=this)
        {
            Instance = this;
             Debug.Log("Instance Initialized");
        }
    }
    private ScoreManager(){

        if (Instance == null|| Instance!=this)
        {
           
            Instance = this;
        }
    }

    public void AddScore(int points)
    {
        score += points;
        Debug.Log("Score: " + score);
    }
}

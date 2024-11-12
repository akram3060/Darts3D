using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText; // UI Text to display score
    private int currentScore = 0;

    public void IncreaseScore(int score)
    {
        currentScore += score;
        scoreText.text =  currentScore.ToString();
    }
}

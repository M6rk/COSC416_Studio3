using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private float score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;

    void Start()
    {
        // Ensure that there is only one instance of GameManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist the GameManager across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void IncrementScore(int amount)
    {
        score += amount;
        scoreText.text = $"Score: {score}";
        Debug.Log("Score: " + score);
    }

    public float GetScore()
    {
        return score;
    }
}
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
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // increment score in canvas UI text 
    public void IncrementScore(int amount)
    {
        score += amount;
        scoreText.text = $"Score: {score}";
        Debug.Log("Coin collected! +1 score");
    }

    public float GetScore()
    {
        return score;
    }
}
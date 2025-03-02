using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotationSpeed = 50f;

    void Update()
    {
        // rotate coin
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // increment score and delete coin when player collides with coin object
        if (other.CompareTag("Player"))
        {
            GameManager.instance.IncrementScore(1);
            Destroy(gameObject);
        }
    }
}
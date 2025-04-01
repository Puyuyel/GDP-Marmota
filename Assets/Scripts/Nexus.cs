using UnityEngine;

public class Nexus : MonoBehaviour
{
    public int health = 100;

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Nexus Health: " + health);

        if (health <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Debug.Log("¡Game Over! El Nexus ha sido destruido.");
        Time.timeScale = 0;
    }
}

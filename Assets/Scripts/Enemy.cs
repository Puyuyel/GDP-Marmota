using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public int health = 50;
    public int damage = 10;

    private Transform nexus;

    private void Start()
    {
        nexus = GameObject.FindGameObjectWithTag("Nexus").transform;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, nexus.position, speed * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Nexus"))
        {
            Nexus nexusScript = collision.GetComponent<Nexus>();

            if(nexusScript != null)
            {
                nexusScript.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}

using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public int health = 50;
    public int damage = 10;

    private int _incomingDamage = 0;
    private Transform _nexus;

    private void Start()
    {
        _nexus = GameObject.FindGameObjectWithTag("Nexus").transform;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _nexus.position, speed * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public bool ReserveDamage(int damage)
    {
        if (_incomingDamage >= health) return false;
        _incomingDamage += damage;
        return true;
    }

    public void ConfirmHit(int amount)
    {
        _incomingDamage -= amount;
        TakeDamage(amount);
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

using UnityEngine;

public class LootPickup : MonoBehaviour
{
    public float pickupRadius = 1.5f; // Mismo radio que el picado

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= pickupRadius && Input.GetKeyDown(KeyCode.E))
        {
            CollectLoot();
        }
    }

    void CollectLoot()
    {
        Debug.Log("¡Cofre recogido!");
        // Aquí puedes añadir inventario, efectos, sonido, etc.
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}

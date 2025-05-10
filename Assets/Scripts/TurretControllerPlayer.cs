using UnityEngine;

public class TorretaControl : MonoBehaviour
{
    public GameObject proyectilPrefab;
    public float fuerzaDisparo = 10f;
    void Update()
    {
        // Obtener posición del mouse en el mundo 2D
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // Importante en 2D

        // Calcular dirección hacia el mouse
        Vector2 direction = (mousePos - transform.position).normalized;

        // Rotar la torreta (apuntando hacia el mouse)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (Input.GetMouseButtonDown(0))
        {
            GameObject proyectil = Instantiate(proyectilPrefab, transform.position, transform.rotation);
            Rigidbody2D rb = proyectil.GetComponent<Rigidbody2D>();
            rb.AddForce(transform.right * fuerzaDisparo, ForceMode2D.Impulse); // "right" es el eje frontal en 2D
        }
    }

   
}


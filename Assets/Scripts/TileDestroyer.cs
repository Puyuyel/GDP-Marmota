using UnityEngine;
using UnityEngine.Tilemaps;

public class TileDestroyer : MonoBehaviour
{
    public Tilemap groundTilemap;
    public float reachDistance = 1.5f;
    public KeyCode destroyKey = KeyCode.Mouse0;
    public float destroyCooldown = 0.2f;

    private float lastDestroyTime = -Mathf.Infinity;
    private MazeGenerator mazeGenerator;

    void Start()
    {
        if (groundTilemap == null)
        {
            groundTilemap = GameObject.Find("GroundTilemap").GetComponent<Tilemap>();
        }

        mazeGenerator = FindFirstObjectByType<MazeGenerator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(destroyKey) && Time.time >= lastDestroyTime + destroyCooldown)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            Vector2 direction = (mouseWorldPos - transform.position).normalized;
            float stepSize = 0.1f;

            for (float dist = 0; dist <= reachDistance; dist += stepSize)
            {
                Vector3 checkPos = transform.position + (Vector3)(direction * dist);
                Vector3Int cell = groundTilemap.WorldToCell(checkPos);

                if (groundTilemap.HasTile(cell))
                {
                    if (mazeGenerator != null && mazeGenerator.protectedTiles.Contains(cell))
                    {
                        // Tile protegido, no destruir
                        break;
                    }

                    groundTilemap.SetTile(cell, null);
                    lastDestroyTime = Time.time;
                    break;
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, reachDistance);
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MazeGenerator : MonoBehaviour
{
    public int mazeWidth = 21;   // Número de celdas (no tiles)
    public int mazeHeight = 21;

    public TileBase wallTile;         // RuleTile que hace las paredes
    public TileBase backgroundTile;   // Solo decora el fondo

    public Tilemap wallTilemap;       // Contiene paredes
    public Tilemap backgroundTilemap; // Contiene fondo

    public GameObject playerPrefab;
    public GameObject lootPrefab;     // Prefab del cofre

    public HashSet<Vector3Int> protectedTiles = new HashSet<Vector3Int>();

    private int[,] maze;
    private int blockSize = 3;

    void Start()
    {
        GenerateMaze();
        DrawMaze();
        PlaceLoot();
        CreateEntrance();
        SpawnPlayerAtEntrance();
        AddColliderBoundaries();
    }

    void GenerateMaze()
    {
        maze = new int[mazeWidth, mazeHeight];
        for (int x = 0; x < mazeWidth; x++)
            for (int y = 0; y < mazeHeight; y++)
                maze[x, y] = 0;

        RecursiveDFS(1, 1);
    }

    void RecursiveDFS(int x, int y)
    {
        maze[x, y] = 1;

        int[] dirs = { 0, 1, 2, 3 };
        Shuffle(dirs);

        foreach (int dir in dirs)
        {
            int dx = 0, dy = 0;
            switch (dir)
            {
                case 0: dx = 0; dy = 2; break;
                case 1: dx = 2; dy = 0; break;
                case 2: dx = 0; dy = -2; break;
                case 3: dx = -2; dy = 0; break;
            }

            int nx = x + dx;
            int ny = y + dy;

            if (nx > 0 && nx < mazeWidth && ny > 0 && ny < mazeHeight && maze[nx, ny] == 0)
            {
                maze[x + dx / 2, y + dy / 2] = 1;
                RecursiveDFS(nx, ny);
            }
        }
    }

    void DrawMaze()
    {
        wallTilemap.ClearAllTiles();
        backgroundTilemap.ClearAllTiles();

        int fullWidth = mazeWidth * blockSize;
        int fullHeight = mazeHeight * blockSize;

        for (int x = 0; x < fullWidth; x++)
            for (int y = 0; y < fullHeight; y++)
                backgroundTilemap.SetTile(new Vector3Int(x, y, 0), backgroundTile);

        for (int x = 0; x < fullWidth; x++)
            for (int y = 0; y < fullHeight; y++)
                wallTilemap.SetTile(new Vector3Int(x, y, 0), wallTile);

        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                if (maze[x, y] == 1)
                {
                    for (int bx = 0; bx < blockSize; bx++)
                    {
                        for (int by = 0; by < blockSize; by++)
                        {
                            int px = x * blockSize + bx;
                            int py = y * blockSize + by;
                            wallTilemap.SetTile(new Vector3Int(px, py, 0), null);
                        }
                    }
                }
            }
        }
    }

    void PlaceLoot()
    {
        List<Vector2Int> walkableCells = new List<Vector2Int>();

        // Recopilar todas las celdas de camino válidas (maze[x, y] == 1)
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                if (maze[x, y] == 1)
                {
                    walkableCells.Add(new Vector2Int(x, y));
                }
            }
        }

        // Barajar las celdas de camino para aleatorizar la elección
        Shuffle(walkableCells);

        int placedLoot = 0;
        int lootCount = (mazeWidth * mazeHeight) / 50;
        int maxLoot = Mathf.Min(lootCount, walkableCells.Count);

        for (int i = 0; i < walkableCells.Count && placedLoot < maxLoot; i++)
        {
            Vector2Int cell = walkableCells[i];
            float worldX = (cell.x + 0.5f) * blockSize;
            float worldY = (cell.y + 0.5f) * blockSize;
            Vector3 spawnPos = new Vector3(worldX, worldY - 1.0f, 0f);

            // Comprobamos si hay un tile justo debajo (para que el cofre no flote)
            Vector3Int belowCell = wallTilemap.WorldToCell(spawnPos + Vector3.down * 1.0f);
            if (wallTilemap.HasTile(belowCell))
            {
                Instantiate(lootPrefab, spawnPos, Quaternion.identity);
                protectedTiles.Add(belowCell);
                placedLoot++;
            }
        }

        Debug.Log($"Cofres generados: {placedLoot}");
    }


    void CreateEntrance()
    {
        int entranceCellX = mazeWidth / 2;
        int entranceCellY = mazeHeight - 1;

        int tileStartX = entranceCellX * blockSize;
        int tileStartY = entranceCellY * blockSize;

        for (int bx = 0; bx < blockSize; bx++)
        {
            for (int by = -blockSize; by < blockSize; by++)
            {
                Vector3Int pos = new Vector3Int(tileStartX + bx, tileStartY + by, 0);
                wallTilemap.SetTile(pos, null);
            }
        }

        maze[entranceCellX, entranceCellY] = 1;
    }

    void SpawnPlayerAtEntrance()
    {
        int entranceCellX = mazeWidth / 2;
        int entranceCellY = mazeHeight - 1;

        float worldX = (entranceCellX + 0.5f) * blockSize;
        float worldY = (entranceCellY + 0.5f) * blockSize;

        Instantiate(playerPrefab, new Vector3(worldX, worldY, 0), Quaternion.identity);
    }

    void Shuffle(int[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (array[i], array[j]) = (array[j], array[i]);
        }
    }

    void Shuffle(List<Vector2Int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
    void AddColliderBoundaries()
    {
        float mazeWidthInUnits = mazeWidth * blockSize;
        float mazeHeightInUnits = mazeHeight * blockSize;

        // Borde superior
        BoxCollider2D topCollider = gameObject.AddComponent<BoxCollider2D>();
        topCollider.size = new Vector2(mazeWidthInUnits, blockSize);
        topCollider.offset = new Vector2(mazeWidthInUnits / 2f, mazeHeightInUnits + blockSize / 2f);

        // Borde inferior
        BoxCollider2D bottomCollider = gameObject.AddComponent<BoxCollider2D>();
        bottomCollider.size = new Vector2(mazeWidthInUnits, blockSize);
        bottomCollider.offset = new Vector2(mazeWidthInUnits / 2f, -blockSize / 2f);

        // Borde izquierdo
        BoxCollider2D leftCollider = gameObject.AddComponent<BoxCollider2D>();
        leftCollider.size = new Vector2(blockSize, mazeHeightInUnits);
        leftCollider.offset = new Vector2(-blockSize / 2f, mazeHeightInUnits / 2f); 

        // Borde derecho
        BoxCollider2D rightCollider = gameObject.AddComponent<BoxCollider2D>();
        rightCollider.size = new Vector2(blockSize, mazeHeightInUnits); 
        rightCollider.offset = new Vector2(mazeWidthInUnits + blockSize / 2f, mazeHeightInUnits / 2f); 
    }
}

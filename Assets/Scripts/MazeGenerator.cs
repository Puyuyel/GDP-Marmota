using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MazeGenerator : MonoBehaviour
{
    public int mazeWidth = 21;   // Número de celdas (no tiles)
    public int mazeHeight = 21;
    
    public TileBase wallTile;         // groundTile: RuleTile que hace las paredes
    public TileBase backgroundTile;   // backgroundTile: solo decora el fondo

    public Tilemap wallTilemap;       // El que contiene paredes
    public Tilemap backgroundTilemap; // El que contiene fondo

    public GameObject playerPrefab;

    private int[,] maze;
    private int blockSize = 3; // Caminos anchos (mínimo 3x3)

    void Start()
    {
        GenerateMaze();
        DrawMaze();
        CreateEntrance();
       // MoveCameraToEntrance();
        SpawnPlayerAtEntrance();
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
                case 0: dx = 0; dy = 2; break;   // Arriba
                case 1: dx = 2; dy = 0; break;   // Derecha
                case 2: dx = 0; dy = -2; break;  // Abajo
                case 3: dx = -2; dy = 0; break;  // Izquierda
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

        // Rellenar todo el fondo
        for (int x = 0; x < fullWidth; x++)
            for (int y = 0; y < fullHeight; y++)
                backgroundTilemap.SetTile(new Vector3Int(x, y, 0), backgroundTile);

        // Rellenar todo con paredes primero (por defecto)
        for (int x = 0; x < fullWidth; x++)
            for (int y = 0; y < fullHeight; y++)
                wallTilemap.SetTile(new Vector3Int(x, y, 0), wallTile);

        // "Borrar" zonas de camino (vaciar paredes para que se vea el fondo)
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
                            wallTilemap.SetTile(new Vector3Int(px, py, 0), null); // borrar pared
                        }
                    }
                }
            }
        }
    }

    void CreateEntrance()
    {
        int entranceCellX = mazeWidth / 2;
        int entranceCellY = mazeHeight - 1;

        int tileStartX = entranceCellX * blockSize;
        int tileStartY = entranceCellY * blockSize;

        for (int bx = 0; bx < blockSize; bx++)
        {
            for (int by = -blockSize; by <  blockSize; by++)
            {
                Vector3Int pos = new Vector3Int(tileStartX + bx, tileStartY + by, 0);
                wallTilemap.SetTile(pos, null); // Eliminar paredes
            }
        }

        // Marcarlo como camino por si quieres usarlo después
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
}

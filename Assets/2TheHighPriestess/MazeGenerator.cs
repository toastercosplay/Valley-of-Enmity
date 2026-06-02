using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MazeGenerator : MonoBehaviour
{
    public int width = 21;
    public int height = 21;

    public int berryamount = 5;
    private int uncollectedAmount;

    public int startX { get; private set; }
    public int startY { get; private set; }

    [SerializeField] List<Sprite> hedge; //wall
    [SerializeField] List<Sprite> floor; //floor
    [SerializeField] GameObject berry; //berry
    [SerializeField] GameObject center; //for center of maze, objective

    private int[,] maze;
    private GameObject[,] spawnedTiles;

    public List <MazePlayer> playerList;
    int maxBerries = 0;
    int minBerries = 100;
    int winnerIndex = 0;
    int loserIndex = 0;

    void Start()
    {
        if (width % 2 == 0) width++;
        if (height % 2 == 0) height++;
        
        maze = new int[width, height];
        spawnedTiles = new GameObject[width, height];

        GenerateMaze();
        ClearCenter();
        SprinkleBerries();
        DrawMaze();

        uncollectedAmount = berryamount;
    }
    
    void Update()
    {
        maxBerries = -1;
        minBerries = int.MaxValue;

        for (int i = 0; i < playerList.Count; i++)
        {
            MazePlayer player = playerList[i];

            if (player.berriesCollected > maxBerries)
            {
                maxBerries = player.berriesCollected;
                winnerIndex = i;
            }
            
            if (player.berriesCollected < minBerries)
            {
                minBerries = player.berriesCollected;
                loserIndex = i;
            }
        }

        if (uncollectedAmount == 0)
        {
            for (int i = 0; i < playerList.Count; i++)
            {
                MazePlayer player = playerList[i];
                player.playerData.SetBufferState(2); 

                if (i == winnerIndex)
                {
                    player.playerData.SetBufferState(1);
                }
                if (i == loserIndex)
                {
                    player.playerData.SetBufferState(3);
                }
            }
            
            GameManager.Instance.FinishMinigame();
        }
    }

    void GenerateMaze()
    {
        if (width % 2 == 0) width--;
        if (height % 2 == 0) height--;

        startX = (Random.value > 0.5f) ? 1 : width - 2;
        startY = (Random.value > 0.5f) ? 1 : height - 2;

        if (startX % 2 == 0) startX--;
        if (startY % 2 == 0) startY--;
        DFS(startX, startY);
    }

    void DFS(int x, int y)
    {
        maze[x, y] = 1;


        Vector2Int[] dirs = 
        {
            new Vector2Int(0, 2), new Vector2Int(0, -2),
            new Vector2Int(2, 0), new Vector2Int(-2, 0)
        };

        ShuffleArray(dirs);

        foreach (var dir in dirs)
        {
            int nx = x + dir.x;
            int ny = y + dir.y;

            if (nx > 0 && nx < width - 1 && ny > 0 && ny < height - 1 && maze[nx, ny] == 0)
            {
                maze[x + dir.x / 2, y + dir.y / 2] = 1;
                DFS(nx, ny);
            }
        }
    }

    void ClearCenter()
    {
        int midX = width / 2;
        int midY = height / 2;

        for (int x = midX - 1; x <= midX + 1; x++)
        {
            for (int y = midY - 1; y <= midY + 1; y++)
            {
                if (x > 0 && x < width - 1 && y > 0 && y < height - 1)
                    maze[x, y] = 3;
            }
        }
    }

    void DrawMaze()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(x, y, 0);
                int tileType = maze[x, y];

                if (tileType == 0) 
                {
                    CreateBackgroundTile(pos, hedge[Random.Range(0, hedge.Count)], 0);
                }
                else 
                {
                    CreateBackgroundTile(pos, floor[Random.Range(0, floor.Count)], -1);
                }


                switch (tileType)
                {
                    case 2: // Berry
                        GameObject berryObj = Instantiate(berry, pos, Quaternion.identity, transform);
                        spawnedTiles[x, y] = berryObj;
                        break;
                    case 3: // Center
                        GameObject centerObj = Instantiate(center, pos, Quaternion.identity, transform);
                        spawnedTiles[x, y] = centerObj;
                        break;
                }
            }
        }
    }

    //
    void CreateBackgroundTile(Vector3 pos, Sprite sprite, int sortingOrder)
    {
        GameObject tile = new GameObject("BackgroundTile");
        tile.transform.position = pos;
        tile.transform.parent = transform;
        SpriteRenderer sr = tile.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.sortingOrder = sortingOrder;
    }

    void ShuffleArray(Vector2Int[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            Vector2Int temp = array[i];
            int randomIndex = Random.Range(i, array.Length);
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    void SprinkleBerries()
    {
      List<Vector2Int> validCells = new List<Vector2Int>();

        // Find all empty path tiles that aren't the start position
        for (int x = 1; x < width - 1; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                if (maze[x, y] == 1 && (x != startX || y != startY))
                {
                    validCells.Add(new Vector2Int(x, y));
                }
            }
        }

        // Randomly pick spots from the list
        for (int i = 0; i < berryamount && validCells.Count > 0; i++)
        {
            int index = Random.Range(0, validCells.Count);
            Vector2Int pos = validCells[index];
            maze[pos.x, pos.y] = 2; // Set to 2 for Berry
            validCells.RemoveAt(index);
        }  
    }

    public int GetValue(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return maze[x, y];
        }
        return 0;
    }

    public void AlterTile(int x, int y, int newValue)
    {
        maze[x,y] = newValue;
    }

    public void CollectBerry(int x, int y)
    {
        if (maze[x, y] == 2) // Confirm it's a berry
        {
            maze[x, y] = 1; // Change logic to floor
            if (spawnedTiles[x, y] != null)
            {
                Destroy(spawnedTiles[x, y]); // Remove the visual berry
            }
        }
    }

    public void UpdateCollection()
    {
        uncollectedAmount -= 1;
    }

}
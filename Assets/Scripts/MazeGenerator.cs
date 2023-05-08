using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public int width, height; // dimensions of the maze
    public GameObject[] roomTemplates; // array of room templates to use in the maze
    public GameObject[] monsterPrefabs; // array of monster prefabs to spawn in the maze
    public GameObject[] chestPrefabs; // array of chest prefabs to spawn in the maze
    public GameObject[] otherObjectPrefabs; // array of other object prefabs to spawn in the maze
    public float monsterFrequency; // frequency of monster spawns in the maze
    public float chestFrequency; // frequency of chest spawns in the maze
    public float otherObjectFrequency; // frequency of other object spawns in the maze

    public int[,] Maze; // 2D array to represent the maze structure
    private List<Vector2> _visitedCells; // list of cells that have been visited during maze generation

    public MazeGenerator(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    // Generate a new maze using the Recursive Backtracking algorithm
    public void GenerateMaze(float difficulty)
    {
        Maze = new int[width, height];
        _visitedCells = new List<Vector2>();

        // Start generating the maze from the top-left corner
        Vector2 currentCell = new Vector2(0, 0);
        _visitedCells.Add(currentCell);

        // Recursive function to carve out passages and create dead-ends in the maze
        RecursiveBacktracking(currentCell);

        // Instantiate room templates to populate the maze
        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
        {
            // Skip cells that are part of the maze structure
            if (Maze[x, y] == 1) continue;

            // Instantiate a random room template in this cell
            int randomIndex = Random.Range(0, roomTemplates.Length);
            GameObject roomPrefab = roomTemplates[randomIndex];
            GameObject newRoom = Instantiate(roomPrefab);
            newRoom.transform.position = new Vector3(x * 10, 0, y * 10); // adjust position based on room dimensions

            // Randomly spawn monsters, chests, and other objects in this room
            if (Random.value < monsterFrequency * difficulty)
            {
                int monsterIndex = Random.Range(0, monsterPrefabs.Length);
                GameObject monsterPrefab = monsterPrefabs[monsterIndex];
                GameObject newMonster = Instantiate(monsterPrefab);
                newMonster.transform.position =
                    new Vector3(x * 10, 0, y * 10); // adjust position based on room dimensions
            }

            if (Random.value < chestFrequency * difficulty)
            {
                int chestIndex = Random.Range(0, chestPrefabs.Length);
                GameObject chestPrefab = chestPrefabs[chestIndex];
                GameObject newChest = Instantiate(chestPrefab);
                newChest.transform.position =
                    new Vector3(x * 10, 0, y * 10); // adjust position based on room dimensions
            }

            if (Random.value < otherObjectFrequency * difficulty)
            {
                int otherObjectIndex = Random.Range(0, otherObjectPrefabs.Length);
                GameObject otherObjectPrefab = otherObjectPrefabs[otherObjectIndex];
                GameObject newOtherObject = Instantiate(otherObjectPrefab);
                newOtherObject.transform.position =
                    new Vector3(x * 10, 0, y * 10); // adjust position based on room dimensions
            }
        }
    }

    // Recursive function to generate the maze using the Recursive Backtracking algorithm
    private void RecursiveBacktracking(Vector2 currentCell)
    {
        // Get a list of unvisited neighboring cells
        List<Vector2> unvisitedNeighbors = GetUnvisitedNeighbors(currentCell);

        // If there are no unvisited neighbors, backtrack to the previous cell in the stack
        if (unvisitedNeighbors.Count == 0) return;

        // Choose a random unvisited neighbor and carve a passage between the cells
        int randomIndex = Random.Range(0, unvisitedNeighbors.Count);
        Vector2 nextCell = unvisitedNeighbors[randomIndex];

        int directionX = (int) (nextCell.x - currentCell.x);
        int directionY = (int) (nextCell.y - currentCell.y);

        if (directionX == 1)
        {
            Maze[(int) currentCell.x, (int) currentCell.y] |= 1; // mark passage to the east
            Maze[(int) nextCell.x, (int) nextCell.y] |= 4; // mark passage to the west
        }
        else if (directionX == -1)
        {
            Maze[(int) currentCell.x, (int) currentCell.y] |= 4; // mark passage to the west
            Maze[(int) nextCell.x, (int) nextCell.y] |= 1; // mark passage to the east
        }
        else if (directionY == 1)
        {
            Maze[(int) currentCell.x, (int) currentCell.y] |= 2; // mark passage to the north
            Maze[(int) nextCell.x, (int) nextCell.y] |= 8; // mark passage to the south
        }
        else if (directionY == -1)
        {
            Maze[(int) currentCell.x, (int) currentCell.y] |= 8; // mark passage to the south
            Maze[(int) nextCell.x, (int) nextCell.y] |= 2; // mark passage to the north
        }

        // Add the next cell to the list of visited cells and recursively continue the maze generation
        _visitedCells.Add(nextCell);
        RecursiveBacktracking(nextCell);
    }

    // Get a list of unvisited neighboring cells for a given cell
    private List<Vector2> GetUnvisitedNeighbors(Vector2 cell)
    {
        List<Vector2> neighbors = new List<Vector2>();

        // Check for unvisited neighbors to the east, west, north, and south of the current cell
        Vector2 eastNeighbor = new Vector2(cell.x + 1, cell.y);
        Vector2 westNeighbor = new Vector2(cell.x - 1, cell.y);
        Vector2 northNeighbor = new Vector2(cell.x, cell.y + 1);
        Vector2 southNeighbor = new Vector2(cell.x, cell.y - 1);

        if (eastNeighbor.x < width && !_visitedCells.Contains(eastNeighbor)) neighbors.Add(eastNeighbor);

        if (westNeighbor.x >= 0 && !_visitedCells.Contains(westNeighbor)) neighbors.Add(westNeighbor);

        if (northNeighbor.y < height && !_visitedCells.Contains(northNeighbor)) neighbors.Add(northNeighbor);

        if (southNeighbor.y >= 0 && !_visitedCells.Contains(southNeighbor)) neighbors.Add(southNeighbor);

        return neighbors;
    }
}
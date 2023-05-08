using UnityEngine;

public class MazeManager : MonoBehaviour
{
    public GameObject floorPrefab;
    public GameObject wallPrefab;
    public GameObject monsterPrefab;
    public GameObject chestPrefab;

    public int width = 10;
    public int height = 10;
    public int difficulty = 1;

    private MazeGenerator _mazeGenerator;

    private void Start()
    {
        // Instantiate a new maze generator and generate a new maze
        _mazeGenerator = wallPrefab.AddComponent<MazeGenerator>();
        _mazeGenerator.GenerateMaze(difficulty);

        // Instantiate maze objects based on the maze data
        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
        {
            // Instantiate a floor tile at the current position
            Instantiate(floorPrefab, new Vector3(x, 0, y), Quaternion.identity);

            // If the current cell has a north wall, instantiate a wall tile to the north
            if (_mazeGenerator.Maze[x, y] % 2 == 0)
                Instantiate(wallPrefab, new Vector3(x, 0.5f, y + 0.5f), Quaternion.Euler(0, 90, 0));

            // If the current cell has an east wall, instantiate a wall tile to the east
            if (_mazeGenerator.Maze[x, y] < 2 || _mazeGenerator.Maze[x, y] > 7)
                Instantiate(wallPrefab, new Vector3(x + 0.5f, 0.5f, y), Quaternion.identity);

            // Instantiate monsters and chests randomly based on the difficulty
            if (Random.Range(0, 10) < difficulty)
                Instantiate(monsterPrefab, new Vector3(x, 0.5f, y), Quaternion.identity);

            if (Random.Range(0, 10) < difficulty)
                Instantiate(chestPrefab, new Vector3(x, 0.5f, y), Quaternion.identity);
        }
    }
}
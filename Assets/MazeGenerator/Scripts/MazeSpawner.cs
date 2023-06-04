using System.Collections.Generic;
using UnityEngine;

//<summary>
//Game object, that creates maze and instantiates it in scene
//</summary>
public class MazeSpawner : MonoBehaviour
{
    public enum MazeGenerationAlgorithm
    {
        PureRecursive,
        RecursiveTree,
        RandomTree,
        OldestTree,
        RecursiveDivision
    }

    public MazeGenerationAlgorithm Algorithm = MazeGenerationAlgorithm.PureRecursive;
    public bool FullRandom;
    public int RandomSeed = 12345;
    public GameObject Floor;
    public GameObject Wall;
    public GameObject Pillar;
    public int Rows = 5;
    public int Columns = 5;
    public float CellWidth = 5;
    public float CellHeight = 5;
    public bool AddGaps = true;
    public GameObject GoalPrefab;
    public int EnemyCount = 3;
    public List<GameObject> EnemyPrefabs = new(); // List to store enemy prefabs
    private BasicMazeGenerator mMazeGenerator;

    private void Start()
    {
        if (!FullRandom) Random.seed = RandomSeed;

        switch (Algorithm)
        {
            case MazeGenerationAlgorithm.PureRecursive:
                mMazeGenerator = new RecursiveMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.RecursiveTree:
                mMazeGenerator = new RecursiveTreeMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.RandomTree:
                mMazeGenerator = new RandomTreeMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.OldestTree:
                mMazeGenerator = new OldestTreeMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.RecursiveDivision:
                mMazeGenerator = new DivisionMazeGenerator(Rows, Columns);
                break;
        }

        mMazeGenerator.GenerateMaze();
        for (var row = 0; row < Rows; row++)
        for (var column = 0; column < Columns; column++)
        {
            var x = column * (CellWidth + (AddGaps ? .2f : 0));
            var z = row * (CellHeight + (AddGaps ? .2f : 0));
            var cell = mMazeGenerator.GetMazeCell(row, column);
            GameObject tmp;
            tmp = Instantiate(Floor, new Vector3(x, 0, z), Quaternion.Euler(0, 0, 0));
            tmp.transform.parent = transform;
            if (cell.WallRight)
            {
                tmp = Instantiate(Wall, new Vector3(x + CellWidth / 2, 0, z) + Wall.transform.position,
                    Quaternion.Euler(0, 90, 0)); // right
                tmp.transform.parent = transform;
            }

            if (cell.WallFront)
            {
                tmp = Instantiate(Wall, new Vector3(x, 0, z + CellHeight / 2) + Wall.transform.position,
                    Quaternion.Euler(0, 0, 0)); // front
                tmp.transform.parent = transform;
            }

            if (cell.WallLeft)
            {
                tmp = Instantiate(Wall, new Vector3(x - CellWidth / 2, 0, z) + Wall.transform.position,
                    Quaternion.Euler(0, 270, 0)); // left
                tmp.transform.parent = transform;
            }

            if (cell.WallBack)
            {
                tmp = Instantiate(Wall, new Vector3(x, 0, z - CellHeight / 2) + Wall.transform.position,
                    Quaternion.Euler(0, 180, 0)); // back
                tmp.transform.parent = transform;
            }

            if (cell.IsGoal && GoalPrefab != null)
            {
                tmp = Instantiate(GoalPrefab, new Vector3(x, 1, z), Quaternion.Euler(0, 0, 0));
                tmp.transform.parent = transform;
            }
        }

        if (Pillar != null)
            for (var row = 0; row < Rows + 1; row++)
            for (var column = 0; column < Columns + 1; column++)
            {
                var x = column * (CellWidth + (AddGaps ? .2f : 0));
                var z = row * (CellHeight + (AddGaps ? .2f : 0));
                var tmp = Instantiate(Pillar, new Vector3(x - CellWidth / 2, 0, z - CellHeight / 2),
                    Quaternion.identity);
                tmp.transform.parent = transform;
            }

        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        // Get the center position of the maze
        var centerX = (Columns - 1) * 0.5f * (CellWidth + (AddGaps ? 0.2f : 0));
        var centerZ = (Rows - 1) * 0.5f * (CellHeight + (AddGaps ? 0.2f : 0));

        // Get the player's starting cell position
        var playerStartRow = 1;
        var playerStartColumn = 1;
        var playerStartX = playerStartColumn * (CellWidth + (AddGaps ? 0.2f : 0));
        var playerStartZ = playerStartRow * (CellHeight + (AddGaps ? 0.2f : 0));

        var enemyCount = 0;

        // Create an array or list of enemy prefabs
        var enemyPrefabs = EnemyPrefabs.ToArray();

        // Shuffle the enemy prefabs array to randomize the selection
        for (var i = 0; i < enemyPrefabs.Length - 1; i++)
        {
            var randomIndex = Random.Range(i, enemyPrefabs.Length);
            (enemyPrefabs[i], enemyPrefabs[randomIndex]) = (enemyPrefabs[randomIndex], enemyPrefabs[i]);
        }

        // Spawn enemies
        while (enemyCount < EnemyCount)
        {
            // Generate random row and column
            var randomRow = Random.Range(0, Rows);
            var randomColumn = Random.Range(0, Columns);

            // Skip the player's starting cell
            if (randomRow == playerStartRow && randomColumn == playerStartColumn)
                continue;

            // Calculate the position of the enemy
            var enemyX = randomColumn * (CellWidth + (AddGaps ? 0.2f : 0));
            var enemyZ = randomRow * (CellHeight + (AddGaps ? 0.2f : 0));

            // Get the enemy prefab based on the current enemy count
            var enemyPrefab = enemyPrefabs[enemyCount % enemyPrefabs.Length];

            // Spawn the enemy
            if (enemyPrefab != null)
            {
                var enemy =
                    Instantiate(enemyPrefab, new Vector3(enemyX, 1, enemyZ), Quaternion.Euler(0, 0, 0));
                enemy.transform.parent = transform;
                enemyCount++;
            }
        }
    }
}
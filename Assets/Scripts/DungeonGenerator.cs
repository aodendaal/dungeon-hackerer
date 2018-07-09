using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public class DungeonGenerator : MonoBehaviour
{
    #region Prefabs

    [Header("1-Exit Building Blocks")]
    [SerializeField]
    private GameObject upPrefab;

    [SerializeField]
    private GameObject downPrefab;

    [SerializeField]
    private GameObject leftPrefab;

    [SerializeField]
    private GameObject rightPrefab;

    [SerializeField]
    private GameObject entranceUpPrefab;

    [SerializeField]
    private GameObject entranceDownPrefab;

    [SerializeField]
    private GameObject entranceLeftPrefab;

    [SerializeField]
    private GameObject entranceRightPrefab;

    [SerializeField]
    private GameObject exitUpPrefab;

    [SerializeField]
    private GameObject exitDownPrefab;

    [SerializeField]
    private GameObject exitLeftPrefab;

    [SerializeField]
    private GameObject exitRightPrefab;

    [Header("2-Building Blocks")]
    [SerializeField]
    private GameObject upLeftPrefab;

    [SerializeField]
    private GameObject upRightPrefab;

    [SerializeField]
    private GameObject upDownPrefab;

    [SerializeField]
    private GameObject downLeftPrefab;

    [SerializeField]
    private GameObject downRightPrefab;

    [SerializeField]
    private GameObject leftRightPrefab;

    [Header("3-Exit Building Blocks")]
    [SerializeField]
    private GameObject upDownRightPrefab;

    [SerializeField]
    private GameObject upDownLeftPrefab;

    [SerializeField]
    private GameObject upLeftRightPrefab;

    [SerializeField]
    private GameObject downLeftRightPrefab;

    [Header("4-Exit Building Blocks")]
    [SerializeField]
    private GameObject upDownLeftRightPrefab;

    [Header("Monsters")]
    [SerializeField]
    private GameObject monsterPrefab;

    #endregion Prefabs

    #region Interface

    public void Generate(int initialState)
    {
        Random.InitState(initialState);

        InitializeDataGrid();

        GenerateGrid();

        GenerateMonsters();

        InstantiateMap(0);
    }


    #endregion Interface

    #region Generation

    private void InitializeDataGrid()
    {
        DungeonData.DataMap = new DataCell[DungeonData.levels][];

        for (var l = 0; l < DungeonData.levels; l++)
        {
            DungeonData.DataMap[l] = new DataCell[DungeonData.gridSize * DungeonData.gridSize];

            for (int x = 0; x < DungeonData.gridSize; x++)
            {
                for (int y = 0; y < DungeonData.gridSize; y++)
                {
                    DungeonData.DataMap[l][y * DungeonData.gridSize + x] = new DataCell { X = x, Y = y };
                }
            }
        }
    }

    private void GenerateGrid()
    {
        for (var level = 0; level < DungeonData.levels; level++)
        {
            var trail = new Stack<Vector2>();
            var path = new Queue<Vector2>();

            path.Enqueue(new Vector2(0, 0));
            trail.Push(new Vector2(0, 0));

            Direction lastDirection = Direction.Down;
            int sameDirectionCount = 0;

            while (trail.Count > 0)
            {
                var location = trail.Peek();

                var choices = new List<Direction>(new Direction[] { Direction.Up, Direction.Left, Direction.Right, Direction.Down });

                if (sameDirectionCount == 1)
                {
                    choices.Remove(lastDirection);
                    sameDirectionCount = 0;
                }

                while (choices.Count > 0)
                {
                    var choice = choices[Random.Range(0, choices.Count)];

                    Vector2 newLocation = new Vector2(0, 0);
                    switch (choice)
                    {
                        case Direction.Up: newLocation = location + new Vector2(0, 1); break;
                        case Direction.Right: newLocation = location + new Vector2(1, 0); break;
                        case Direction.Down: newLocation = location + new Vector2(0, -1); break;
                        case Direction.Left: newLocation = location + new Vector2(-1, 0); break;
                    }

                    if (newLocation.x >= 0 && newLocation.x < DungeonData.gridSize &&
                        newLocation.y >= 0 && newLocation.y < DungeonData.gridSize &&
                        !path.Contains(newLocation))
                    {
                        if (choice == lastDirection)
                        {
                            sameDirectionCount++;
                        }
                        else
                        {
                            lastDirection = choice;
                            sameDirectionCount = 0;
                        }

                        var x = (int)location.x;
                        var y = (int)location.y;

                        switch (choice)
                        {
                            case Direction.Up:
                                DungeonData.DataMap[level][y * DungeonData.gridSize + x].GoUp = true;
                                DungeonData.DataMap[level][(y + 1) * DungeonData.gridSize + x].GoDown = true;
                                break;

                            case Direction.Right:
                                DungeonData.DataMap[level][y * DungeonData.gridSize + x].GoRight = true;
                                DungeonData.DataMap[level][y * DungeonData.gridSize + (x + 1)].GoLeft = true;
                                break;

                            case Direction.Down:
                                DungeonData.DataMap[level][y * DungeonData.gridSize + x].GoDown = true;
                                DungeonData.DataMap[level][(y - 1) * DungeonData.gridSize + x].GoUp = true;
                                break;

                            case Direction.Left:
                                DungeonData.DataMap[level][y * DungeonData.gridSize + x].GoLeft = true;
                                DungeonData.DataMap[level][y * DungeonData.gridSize + (x - 1)].GoRight = true;
                                break;
                        }

                        path.Enqueue(newLocation);
                        trail.Push(newLocation);
                        break;
                    }

                    choices.Remove(choice);
                }

                if (choices.Count == 0)
                {
                    trail.Pop();
                }
            }

            // Add Bridge - Up/Down
            while (true)
            {
                var x = Random.Range(0, DungeonData.gridSize);
                var y = Random.Range(0, DungeonData.gridSize - 1);

                if (!DungeonData.DataMap[level][y * DungeonData.gridSize + x].GoUp)
                {
                    DungeonData.DataMap[level][y * DungeonData.gridSize + x].GoUp = true;
                    DungeonData.DataMap[level][(y + 1) * DungeonData.gridSize + x].GoDown = true;

                    break;
                }
            }

            // Add Bridge - Up/Down
            while (true)
            {
                var x = Random.Range(0, DungeonData.gridSize);
                var y = Random.Range(1, DungeonData.gridSize);

                if (!DungeonData.DataMap[level][y * DungeonData.gridSize + x].GoDown)
                {
                    DungeonData.DataMap[level][y * DungeonData.gridSize + x].GoDown = true;
                    DungeonData.DataMap[level][(y - 1) * DungeonData.gridSize + x].GoUp = true;

                    break;
                }
            }

            // Add Bridge - Left/Right
            while (true)
            {
                var x = Random.Range(1, DungeonData.gridSize);
                var y = Random.Range(0, DungeonData.gridSize);

                if (!DungeonData.DataMap[level][y * DungeonData.gridSize + x].GoLeft)
                {
                    DungeonData.DataMap[level][y * DungeonData.gridSize + x].GoLeft = true;
                    DungeonData.DataMap[level][y * DungeonData.gridSize + (x - 1)].GoRight = true;

                    break;
                }
            }

            // Add Bridge - Left/Right
            while (true)
            {
                var x = Random.Range(0, DungeonData.gridSize - 1);
                var y = Random.Range(0, DungeonData.gridSize);

                if (!DungeonData.DataMap[level][y * DungeonData.gridSize + x].GoRight)
                {
                    DungeonData.DataMap[level][y * DungeonData.gridSize + x].GoRight = true;
                    DungeonData.DataMap[level][y * DungeonData.gridSize + (x + 1)].GoLeft = true;

                    break;
                }
            }

            // Determine Entrance
            var oneExits = DungeonData.DataMap[level].Where(cell => cell.TileType == 1 || cell.TileType == 2 || cell.TileType == 4 || cell.TileType == 8).ToList();

            var index = Random.Range(0, oneExits.Count);
            oneExits[index].IsEntrance = true;
            oneExits.RemoveAt(index);

            // Determine Exit
            if (level < DungeonData.levels - 1)
            {
                index = Random.Range(0, oneExits.Count);
                oneExits[index].IsExit = true;
            }
        }
    }

    private void GenerateMonsters()
    {
        for (var level = 0; level < DungeonData.levels; level++)
        {
            for (var i = 0; i < 8; i++)
            {
                var cells = DungeonData.DataMap[level].Where(cellData => !cellData.IsEntrance && !cellData.IsExit && !cellData.IsOccupied).ToArray();
                var cell = cells[Random.Range(0, cells.Length)];

                cell.IsOccupied = true;
            }
        }
    }

    #endregion Generation

    #region Instantiation
    public void Clear()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    public void InstantiateMap(int level)
    {
        for (int x = 0; x < DungeonData.gridSize; x++)
        {
            for (int y = 0; y < DungeonData.gridSize; y++)
            {
                GameObject prefab;

                var cell = DungeonData.DataMap[level][y * DungeonData.gridSize + x];

                // 0-Exits
                if (!cell.GoUp && !cell.GoDown && !cell.GoLeft && !cell.GoRight)
                {
                    prefab = null;
                }
                // 1-Exit
                else if (!cell.GoUp && cell.GoDown && !cell.GoLeft && !cell.GoRight)
                {
                    if (cell.IsEntrance)
                        prefab = entranceDownPrefab;
                    else if (cell.IsExit)
                        prefab = exitDownPrefab;
                    else
                        prefab = downPrefab;
                }
                else if (cell.GoUp && !cell.GoDown && !cell.GoLeft && !cell.GoRight)
                {
                    if (cell.IsEntrance)
                        prefab = entranceUpPrefab;
                    else if (cell.IsExit)
                        prefab = exitUpPrefab;
                    else
                        prefab = upPrefab;
                }
                else if (!cell.GoUp && !cell.GoDown && cell.GoLeft && !cell.GoRight)
                {
                    if (cell.IsEntrance)
                        prefab = entranceLeftPrefab;
                    else if (cell.IsExit)
                        prefab = exitLeftPrefab;
                    else
                        prefab = leftPrefab;
                }
                else if (!cell.GoUp && !cell.GoDown && !cell.GoLeft && cell.GoRight)
                {
                    if (cell.IsEntrance)
                        prefab = entranceRightPrefab;
                    else if (cell.IsExit)
                        prefab = exitRightPrefab;
                    else
                        prefab = rightPrefab;
                }

                // 2-Exits
                else if (cell.GoUp && !cell.GoDown && cell.GoLeft && !cell.GoRight)
                {
                    prefab = upLeftPrefab;
                }
                else if (cell.GoUp && !cell.GoDown && !cell.GoLeft && cell.GoRight)
                {
                    prefab = upRightPrefab;
                }
                else if (cell.GoUp && cell.GoDown && !cell.GoLeft && !cell.GoRight)
                {
                    prefab = upDownPrefab;
                }
                else if (!cell.GoUp && cell.GoDown && cell.GoLeft && !cell.GoRight)
                {
                    prefab = downLeftPrefab;
                }
                else if (!cell.GoUp && cell.GoDown && !cell.GoLeft && cell.GoRight)
                {
                    prefab = downRightPrefab;
                }
                else if (!cell.GoUp && !cell.GoDown && cell.GoLeft && cell.GoRight)
                {
                    prefab = leftRightPrefab;
                }
                // 3-Exits
                else if (cell.GoUp && !cell.GoDown && cell.GoLeft && cell.GoRight)
                {
                    prefab = upLeftRightPrefab;
                }
                else if (!cell.GoUp && cell.GoDown && cell.GoLeft && cell.GoRight)
                {
                    prefab = downLeftRightPrefab;
                }
                else if (cell.GoUp && cell.GoDown && cell.GoLeft && !cell.GoRight)
                {
                    prefab = upDownLeftPrefab;
                }
                else if (cell.GoUp && cell.GoDown && !cell.GoLeft && cell.GoRight)
                {
                    prefab = upDownRightPrefab;
                }
                // 4-Exits
                else
                {
                    prefab = upDownLeftRightPrefab;
                }

                if (prefab != null)
                {
                    var go = Instantiate(prefab, new Vector3(x * 12, 0, y * 12), Quaternion.identity);
                    go.transform.parent = gameObject.transform;
                    go.name = string.Format("Block ({0}, {1})", cell.X, cell.Y);
                }

                if (cell.IsOccupied)
                {
                    Instantiate(monsterPrefab, new Vector3(cell.X * 12.0f + 2.0f, 0f, cell.Y * 12.0f - 6.0f), Quaternion.AngleAxis(180f, Vector3.up));
                }
            }
        }
    }

    #endregion Instantiation
}
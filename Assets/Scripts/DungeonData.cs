using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class DungeonData
{
    public static event EventHandler OnPlayerMoved;

    public static DataCell[][] DataMap;

    public static int gridSize = 8;
    public static int levels = 10;

    private static Vector3 playerPosition;
    public static Vector3 PlayerPosition
    {
        get { return playerPosition; }
        set
        {
            playerPosition = value;

            if (OnPlayerMoved != null)
            {
                OnPlayerMoved(null, new EventArgs());
            }
        }

    }

    public static int currentLevel = 0;

    public static bool inDebugMode = false;

    public static Vector2 ConvertToMapCoord(Vector3 position)
    {
        var result = new Vector2();

        result.x = Mathf.RoundToInt((position.x - 2.0f) / 12.0f);
        result.y = Mathf.RoundToInt((position.z + 6.0f) / 12.0f);

        return result;
    }

    public static Vector3 ConvertToWorldCoord(Vector2 position)
    {
        return new Vector3(position.x * 12.0f + 2.0f, 0f, position.y * 12.0f - 6.0f);
    }

    public static Vector3 GetEntrancePosition(int level)
    {
        var entrance = DataMap[level].First(cell => cell.IsEntrance);

        //Debug.Log(string.Format("Entrance ({0}, {1})", entrance.X, entrance.Y));

        return new Vector3(entrance.X * 12.0f + 2.0f, 0f, entrance.Y * 12.0f - 6.0f);
    }    

    public static float GetEntranceDirection(int level)
    {
        var entrance = DataMap[level].First(cell => cell.IsEntrance);
        if (entrance.TileType == 1)
        {
            return 0f;
        }
        else if (entrance.TileType == 2)
        {
            return 180f;
        }
        else if (entrance.TileType == 4)
        {
            return 270f;
        }
        else if (entrance.TileType == 8)
        {
            return 90f;
        }

        return 0;
    }

    public static Vector3 GetExitPosition(int level)
    {
        var entrance = DataMap[level].First(cell => cell.IsExit);

        //Debug.Log(string.Format("Exit ({0}, {1})", entrance.X, entrance.Y));

        return new Vector3(entrance.X * 12.0f + 2.0f, 0f, entrance.Y * 12.0f - 6.0f);
    }
}

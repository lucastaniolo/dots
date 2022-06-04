using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static T GetRandom<T>(this IList<T> list) => list[Random.Range(0, list.Count)];

    public static bool IsAdjacent(this Vector2Int current, Vector2Int target)
    {
        var horizontalGridDistance = Mathf.Abs(current.x - target.x);
        var verticalGridDistance = Mathf.Abs(current.y - target.y);
        
        return horizontalGridDistance + verticalGridDistance == 1;
    }
}
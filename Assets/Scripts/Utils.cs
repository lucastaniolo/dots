using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static T GetRandom<T>(this IList<T> list) => list[Random.Range(0, list.Count)];
}
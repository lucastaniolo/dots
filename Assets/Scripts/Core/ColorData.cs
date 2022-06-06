using System;
using UnityEngine;

[Serializable]
public class ColorData
{
    public Color Color;
    public int ColorId;
}

[Serializable]
public class DotData
{
    public ColorData ColorData;
    public Vector2[,] GridPositions;
    public Vector2Int GridIndex;

    public DotData(ColorData colorData, Vector2Int gridIndex, ref Vector2[,] gridPositions)
    {
        ColorData = colorData;
        GridIndex = gridIndex;
        GridPositions = gridPositions;
    }

    // Caching GridPositions adds the possibility to react to global events independently
    public Vector2 GridPosition => GridPositions[GridIndex.x, GridIndex.y];
}
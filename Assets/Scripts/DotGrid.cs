using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DotGrid : MonoBehaviour
{
    [SerializeField] private Transform gridContainer;
    
    [SerializeField] private int rows;
    [SerializeField] private int columns;
    [SerializeField] private float spacing = 0.8f;

    [SerializeField] private Dot dotPrefab;
    
    // TODO center on camera position
    // TODO update camera size based on grid size
    [SerializeField] private Transform anchor;

    private Dot[,] Dots { get; set; }
    
    private Vector2[,] gridPositions;
    private Vector2 anchorPos;

    private void Awake()
    {
        Dots = new Dot[rows, columns];
        gridPositions = new Vector2[rows, columns];
        anchorPos = anchor.position;
    }
    
    private void Start()
    {
        CreateGrid();
    }
    
    private void CreateGrid()
    {
        for (var x = 0; x < rows; x++)
        {
            for (var y = 0; y < columns; y++)
            {
                gridPositions[x, y] = new Vector2(anchorPos.x + x * spacing, anchorPos.y - y * spacing);
                var dot = Instantiate(dotPrefab, gridContainer);
                var data = new DotData(GamePalette.Instance.GetRandomColor(), new Vector2Int(x, y), ref gridPositions);
                
                dot.SetupData(data);
                dot.MoveTo();
                Dots[x, y] = dot;
            }
        }
    }

    public void DisableDots(List<DotData> data)
    {
        foreach (var d in data)
            Dots[d.GridIndex.x, d.GridIndex.y].Disable();
    }
    
    public void Reorder()
    {
        for (var x = rows - 1; x >= 0; x--)
        {
            for (var y = columns - 1; y >= 0; y--)
            {
                // Look for empty dots
                if (Dots[x, y].IsActive) continue;
                
                // Bubble sort this column from the bottom up trying to find
                // an active dot to swap for the inactive one
                for (var i = y - 1; i >= 0; i--)
                {
                    if (!Dots[x, i].IsActive) continue;
                    
                    SwapVerticalDots(x, y, i);
                    break;
                }
                
                if (!Dots[x, y].IsActive)
                    Dots[x, y].Reuse();
            }
        }
    }

    private void SwapVerticalDots(int x, int a, int b)
    {
        // Swap grid references
        (Dots[x, a], Dots[x, b]) = (Dots[x, b], Dots[x, a]);
        
        // Update local grid info
        (Dots[x, a].Data.GridIndex, Dots[x, b].Data.GridIndex) = (Dots[x, b].Data.GridIndex, Dots[x, a].Data.GridIndex);
    }
    
    public List<DotData> GetDotsByColor(ColorData colorData)
    {
        return Dots
            .Cast<Dot>()
            .Where(dot => dot.Data.ColorData.ColorId == colorData.ColorId)
            .Select(d => d.Data).ToList();
    }

    public void Animate()
    {
        foreach (var dot in Dots)
            dot.MoveTo();
    }
}
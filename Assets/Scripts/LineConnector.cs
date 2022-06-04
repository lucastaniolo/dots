using System.Collections.Generic;
using UnityEngine;

public class LineConnector : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private DotInputHandler inputHandler;
    
    private void OnEnable()
    {
        inputHandler.DotSelectedEvent += AddLinePoint;
        inputHandler.SelectionEndedEvent += RemovePoints;
    }

    private void OnDisable()
    {
        inputHandler.DotSelectedEvent -= AddLinePoint;
        inputHandler.SelectionEndedEvent -= RemovePoints;
    }
    
    private void SetColor(Color color)
    {
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }

    private void AddLinePoint(Dot dot)
    {
        if (lineRenderer.positionCount == 0)
        {
            SetColor(dot.Data.ColorData.Color);
        }
        
        lineRenderer.SetPosition(lineRenderer.positionCount++, dot.Data.GridPosition);
    }

    private void RemovePoints(List<Dot> _)
    {
        lineRenderer.positionCount = 0;
    }
}

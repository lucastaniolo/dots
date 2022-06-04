using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LineConnector : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private DotInputHandler inputHandler;
    
    private void OnEnable()
    {
        inputHandler.DotSelectedEvent += AddLinePoint;
        inputHandler.SelectionEndedEvent += RemovePoints;
        inputHandler.SelectionDraggingEvent += OnDrag;
    }
    
    private void OnDisable()
    {
        inputHandler.DotSelectedEvent -= AddLinePoint;
        inputHandler.SelectionEndedEvent -= RemovePoints;
        inputHandler.SelectionDraggingEvent -= OnDrag;
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
            lineRenderer.positionCount++;
            SetColor(dot.Data.ColorData.Color);
        }
        
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, dot.Data.GridPosition);
    }

    private void RemovePoints(List<Dot> _)
    {
        lineRenderer.positionCount = 0;
    }
    
    private void OnDrag(int selectedDotsCount, Vector2 position)
    {
        if (selectedDotsCount != lineRenderer.positionCount - 1)
            lineRenderer.positionCount = selectedDotsCount + 1;
        
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, position);
    }
}

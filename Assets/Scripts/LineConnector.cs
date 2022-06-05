using System.Collections.Generic;
using UnityEngine;

public class LineConnector : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private LineRenderer dragLineRenderer;
    [SerializeField] private DotInputHandler inputHandler;
    
    private void OnEnable()
    {
        inputHandler.DotSelectedEvent += AddLinePoint;
        inputHandler.SelectionEndedEvent += RemovePoints;
        inputHandler.SelectionDraggingEvent += OnDrag;
        inputHandler.DotUnselectedEvent += DisconnectPoint;
    }

    private void OnDisable()
    {
        inputHandler.DotSelectedEvent -= AddLinePoint;
        inputHandler.SelectionEndedEvent -= RemovePoints;
        inputHandler.SelectionDraggingEvent -= OnDrag;
        inputHandler.DotUnselectedEvent -= DisconnectPoint;
    }
    
    private void SetColor(Color color)
    {
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        dragLineRenderer.startColor = color;
        dragLineRenderer.endColor = color;
    }

    private void AddLinePoint(Dot dot)
    {
        if (lineRenderer.positionCount == 0)
        {
            SetColor(dot.Data.ColorData.Color);
        }
        
        lineRenderer.SetPosition(lineRenderer.positionCount++, dot.Data.GridPosition);
    }

    private void DisconnectPoint(Dot _)
    {
        lineRenderer.positionCount--;
    }
    
    private void RemovePoints(List<Dot> _)
    {
        lineRenderer.positionCount = 0;
        dragLineRenderer.positionCount = 0;
    }
    
    private void OnDrag(int selectedDotsCount, Vector2 position)
    {
        dragLineRenderer.positionCount = 2;
        dragLineRenderer.SetPosition(0, lineRenderer.GetPosition(lineRenderer.positionCount - 1));
        dragLineRenderer.SetPosition(1, position);
    }
}

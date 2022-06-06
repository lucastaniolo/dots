using System.Collections.Generic;
using UnityEngine;

public class LineConnector : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private LineRenderer dragLineRenderer;

    private void OnEnable()
    {
        GameStateHandler.DotsSelectedEvent += AddLinePoint;
        GameStateHandler.DotUnselectedEvent += DisconnectLastPoint;

        GameStateHandler.ClearSelectionEvent += DisconnectAllPoints;
        
        GameStateHandler.SelectionDraggingEvent += UpdateDragLine;
    }

    private void OnDisable()
    {
        GameStateHandler.DotsSelectedEvent -= AddLinePoint;
        GameStateHandler.DotUnselectedEvent -= DisconnectLastPoint;
        
        GameStateHandler.ClearSelectionEvent -= DisconnectAllPoints;
        
        GameStateHandler.SelectionDraggingEvent -= UpdateDragLine;
    }

    private void SetColor(Color color)
    {
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        dragLineRenderer.startColor = color;
        dragLineRenderer.endColor = color;
    }

    private void AddLinePoint(List<DotData> data)
    {
        if (data.Count > 1) return;
        
        var lastDost = data[0];

        if (lineRenderer.positionCount == 0)
            SetColor(lastDost.ColorData.Color);
        
        lineRenderer.SetPosition(lineRenderer.positionCount++, lastDost.GridPosition);
    }

    private void DisconnectLastPoint()
    {
        lineRenderer.positionCount--;
    }
    
    private void DisconnectAllPoints(List<DotData> _)
    {
        lineRenderer.positionCount = 0;
        dragLineRenderer.positionCount = 0;
    }
    
    private void DisconnectAllPoints() => DisconnectAllPoints(null);

    private void UpdateDragLine(int selectedDotsCount, Vector2 position)
    {
        dragLineRenderer.positionCount = 2;
        dragLineRenderer.SetPosition(0, lineRenderer.GetPosition(lineRenderer.positionCount - 1));
        dragLineRenderer.SetPosition(1, position);
    }
}

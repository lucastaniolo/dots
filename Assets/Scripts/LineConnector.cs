using System.Collections.Generic;
using UnityEngine;

public class LineConnector : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private LineRenderer dragLineRenderer;
    
    // TODO Receive notifications through GameManager instead
    [SerializeField] private DotInputHandler inputHandler;
    
    private void OnEnable()
    {
        inputHandler.DotSelectedEvent += AddLinePoint;
        inputHandler.SelectionEndedEvent += RemovePoints;
        inputHandler.SelectionDraggingEvent += OnDrag;
        inputHandler.DotUnselectedEvent += DisconnectPoint;
        GameManager.SquareSuccessEvent += RemovePoints;
    }

    private void OnDisable()
    {
        inputHandler.DotSelectedEvent -= AddLinePoint;
        inputHandler.SelectionEndedEvent -= RemovePoints;
        inputHandler.SelectionDraggingEvent -= OnDrag;
        inputHandler.DotUnselectedEvent -= DisconnectPoint;
        GameManager.SquareSuccessEvent -= RemovePoints;
    }
    
    private void SetColor(Color color)
    {
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        dragLineRenderer.startColor = color;
        dragLineRenderer.endColor = color;
    }

    private void AddLinePoint(DotData data)
    {
        if (lineRenderer.positionCount == 0)
        {
            SetColor(data.ColorData.Color);
        }
        
        lineRenderer.SetPosition(lineRenderer.positionCount++, data.GridPosition);
    }

    private void DisconnectPoint(DotData _)
    {
        lineRenderer.positionCount--;
    }
    
    private void RemovePoints(List<DotData> _)
    {
        lineRenderer.positionCount = 0;
        dragLineRenderer.positionCount = 0;
    }
    
    private void RemovePoints() => RemovePoints(null);

    private void OnDrag(int selectedDotsCount, Vector2 position)
    {
        dragLineRenderer.positionCount = 2;
        dragLineRenderer.SetPosition(0, lineRenderer.GetPosition(lineRenderer.positionCount - 1));
        dragLineRenderer.SetPosition(1, position);
    }
}

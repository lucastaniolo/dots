using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class DotInputHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private BoxCollider2D inputCollider;
    [SerializeField] private float dragThreshold;

    public event Action SelectionStartedEvent;
    public event Action<List<DotData>> SelectionEndedEvent;
    public event Action<DotData> DotSelectedEvent;
    public event Action<DotData> DotUnselectedEvent;
    public event Action<int, Vector2> SelectionDraggingEvent;
    public event Action<DotData> SquarePreSelectEvent;
    public event Action SquareSuccessEvent;

    private readonly List<DotData> selectedDots = new();

    private DotData lastDotSelected;
    
    private int colorId;

    private bool squareSelected;
    
    // TODO States ASAP
    public void OnDrag(PointerEventData eventData)
    {
        if (!eventData.dragging || !eventData.pointerEnter) return;

        if (selectedDots.Count > 0)
        {
            var worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
            if (Vector2.Distance(lastDotSelected.GridPosition, worldPos) > dragThreshold)
                SelectionDraggingEvent?.Invoke(selectedDots.Count, worldPos);
        }
        
        if (eventData.pointerEnter.TryGetComponent<Dot>(out var dot))
        {
            var dotData = dot.Data;
            
            if (selectedDots.Contains(dotData))
            {
                if (selectedDots.Count >= 2 && dotData == selectedDots[^2])
                {
                    var unselectedDot = selectedDots[^1];
                    selectedDots.RemoveAt(selectedDots.Count - 1);
                    DotUnselectedEvent?.Invoke(unselectedDot);
                    lastDotSelected = selectedDots[^1];

                    squareSelected = selectedDots.Count != selectedDots.Distinct().Count();
                    return;
                }
            }

            //Connection Rules
            if (selectedDots.Count == 0)
            {
                colorId = dotData.ColorData.ColorId;
                Select();
            }
            else if (dotData.ColorData.ColorId == colorId && 
                     dotData.GridIndex.IsAdjacent(lastDotSelected.GridIndex))
            {
                Select();
            }

            void Select()
            {
                if (squareSelected &&
                    dotData.GridIndex.IsAdjacent(lastDotSelected.GridIndex) &&
                    selectedDots.Contains(dotData))
                    return;
                
                // Validate SQUARE mechanic
                if (selectedDots.Contains(dotData) &&
                    selectedDots.Count >= 2 &&
                    dotData != selectedDots[^2])
                {
                    SquarePreSelectEvent?.Invoke(dotData);
                    squareSelected = true;
                }
                
                lastDotSelected = dotData;
                selectedDots.Add(lastDotSelected);
                DotSelectedEvent?.Invoke(lastDotSelected);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        inputCollider.transform.position += Vector3.forward;
        SelectionStartedEvent?.Invoke();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        inputCollider.transform.position -= Vector3.forward;

        if (squareSelected)
        {
            SquareSuccessEvent?.Invoke();
            squareSelected = false;
        }
        else
        {
            // Single dot selection is not a correct move
            if (selectedDots.Count == 1)
                selectedDots.Clear();

            SelectionEndedEvent?.Invoke(selectedDots);
        }
        
        selectedDots.Clear();
    }
}
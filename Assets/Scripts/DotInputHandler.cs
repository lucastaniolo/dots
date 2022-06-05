using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DotInputHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private BoxCollider2D inputCollider;
    [SerializeField] private float dragThreshold;

    public event Action SelectionStartedEvent;
    public event Action<List<Dot>> SelectionEndedEvent;
    public event Action<Dot> DotSelectedEvent;
    public event Action<Dot> DotUnselectedEvent;
    public event Action<int, Vector2> SelectionDraggingEvent;

    private readonly List<Dot> selectedDots = new();

    private Dot lastDotSelected;
    
    private int colorId;
    
    public void OnDrag(PointerEventData eventData)
    {
        if (!eventData.dragging || !eventData.pointerEnter) return;

        if (selectedDots.Count > 0)
        {
            var worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
            if (Vector2.Distance(lastDotSelected.Data.GridPosition, worldPos) > dragThreshold)
                SelectionDraggingEvent?.Invoke(selectedDots.Count, worldPos);
        }
        
        if (eventData.pointerEnter.TryGetComponent<Dot>(out var dot))
        {
            if (selectedDots.Contains(dot))
            {
                if (selectedDots.Count <= 1 || selectedDots[^2] != dot) return;

                var unselectedDot = selectedDots[^1];
                selectedDots.Remove(unselectedDot);
                DotUnselectedEvent?.Invoke(unselectedDot);
                lastDotSelected = selectedDots[^1];
                return;
            }

            //Connection Rules
            if (selectedDots.Count == 0)
            {
                colorId = dot.Data.ColorData.ColorId;
                Select();
            }
            else if (dot.Data.ColorData.ColorId == colorId && 
                     dot.Data.GridIndex.IsAdjacent(lastDotSelected.Data.GridIndex))
            {
                Select();
            }

            void Select()
            {
                lastDotSelected = dot;
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
        SelectionEndedEvent?.Invoke(selectedDots);
        selectedDots.Clear();
    }
}
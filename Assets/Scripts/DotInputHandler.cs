using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class DotInputHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private BoxCollider2D inputCollider;

    public event Action SelectionStartedEvent;
    public event Action<List<Dot>> SelectionEndedEvent;
    public event Action<Dot> DotSelectedEvent;
    public event Action<Dot> DotUnselectedEvent;

    private readonly List<Dot> selectedDots = new();

    private int colorId;

    private GameObject current;

    public void OnDrag(PointerEventData eventData)
    {
        if (!eventData.dragging || !eventData.pointerEnter) return;

        if (eventData.pointerEnter.TryGetComponent<Dot>(out var dot))
        {
            if (selectedDots.Contains(dot))
            {
                //TODO Disconnection
                // selectedDots.Remove(dot);
                // DotUnselectedEvent?.Invoke(dot);
            }
            else
            {
                // if (current == dot.gameObject) return;
                
                //Connection Rules
                if (selectedDots.Count == 0)
                {
                    colorId = dot.Data.ColorData.ColorId;
                    Select();
                }
                else if (dot.Data.ColorData.ColorId == colorId && 
                         dot.Data.GridIndex.IsAdjacent(selectedDots.Last().Data.GridIndex))
                {
                    Select();
                }

                void Select()
                {
                    selectedDots.Add(dot);
                    DotSelectedEvent?.Invoke(dot);
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        print("begin drag");

        inputCollider.transform.position += Vector3.forward;
        SelectionStartedEvent?.Invoke();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        print("end drag");
        inputCollider.transform.position -= Vector3.forward;
        SelectionEndedEvent?.Invoke(selectedDots);
        selectedDots.Clear();
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private DotGrid grid;
    [SerializeField] private DotInputHandler inputHandler;

    public enum States
    {
        Idle,
        Select,
        Replenish
    }

    // TODO Improve states
    private States state;
    public States State
    {
        get => state;
        private set
        {
            state = value;
            OnStateUpdated();
        }
    }

    private List<DotData> selectedDots;

    public static event Action<List<DotData>> DotsSelectedEvent;
    public static event Action<DotData> SquaredDotsEvent;
    public static event Action SquareSuccessEvent;
    public static event Action<List<DotData>> ReplenishEvent;

    private void OnEnable()
    {
        inputHandler.SelectionStartedEvent += OnStartSelection;
        inputHandler.SelectionEndedEvent += OnEndSelection;
        inputHandler.DotSelectedEvent += OnDotSelected;
        inputHandler.SquarePreSelectEvent += OnPreSelectSquare;
        inputHandler.SquareSuccessEvent += SquareFinishedEvent;
    }
    
    private void OnDisable()
    {
        inputHandler.SelectionStartedEvent -= OnStartSelection;
        inputHandler.SelectionEndedEvent -= OnEndSelection;
        inputHandler.DotSelectedEvent -= OnDotSelected;
        inputHandler.SquarePreSelectEvent -= OnPreSelectSquare;
        inputHandler.SquareSuccessEvent -= SquareFinishedEvent;
    }

    private void SquareFinishedEvent()
    {
        // SquaredDotsEvent?.Invoke(selectedDots);
        SquareSuccessEvent?.Invoke();
        State = States.Replenish;
    }

    private void OnPreSelectSquare(DotData dotData)
    {
        selectedDots = grid.GetDotsByColor(dotData.ColorData);
        NotifyDotsSelection(selectedDots);
        SquaredDotsEvent?.Invoke(dotData);
    }

    private void OnStartSelection()
    {
        if (State != States.Idle) return;
        
        State = States.Select;
    }
    
    private void OnDotSelected(DotData data)
    {
        NotifyDotsSelection(new List<DotData> { data });
    }

    private void NotifyDotsSelection(List<DotData> data)
    {
        DotsSelectedEvent?.Invoke(data);
    }
    
    private void OnEndSelection(List<DotData> data)
    {
        if (data.Count == 0)
        {
            State = States.Idle;
        }
        else
        {
            selectedDots = data;
            State = States.Replenish;
        }
    }

    private void OnStateUpdated()
    {
        switch (State)
        {
            case States.Idle:
                break;
            case States.Select:
                break;
            case States.Replenish:
                ReplenishEvent?.Invoke(selectedDots);
                grid.DisableDots(selectedDots);
                grid.Reorder();
                grid.AnimateDroppingDots();
                selectedDots = null;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
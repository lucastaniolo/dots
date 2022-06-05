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

    private List<Dot> selectedDots;

    public static event Action<DotData> DotSelectedEvent;
    public static event Action<DotData> DotUnselectedEvent;

    private void OnEnable()
    {
        inputHandler.SelectionStartedEvent += OnStartSelection;
        inputHandler.SelectionEndedEvent += OnEndSelection;
        inputHandler.DotSelectedEvent += OnDotSelected;
        // inputHandler.DotUnselectedEvent += OnDotUnselected;
    }
    
    private void OnDisable()
    {
        inputHandler.SelectionStartedEvent -= OnStartSelection;
        inputHandler.SelectionEndedEvent -= OnEndSelection;
        inputHandler.DotSelectedEvent -= OnDotSelected;
        // inputHandler.DotUnselectedEvent -= OnDotUnselected;
    }

    private void OnStartSelection()
    {
        if (State != States.Idle) return;
        
        State = States.Select;
    }
    
    private void OnDotUnselected(Dot dot)
    {
        // DotUnselectedEvent?.Invoke(dot.Data);
    }

    private void OnDotSelected(Dot dot)
    {
        DotSelectedEvent?.Invoke(dot.Data);
    }
    
    private void OnEndSelection(List<Dot> dots)
    {
        if (dots.Count == 0)
        {
            State = States.Idle;
        }
        else
        {
            selectedDots = dots;
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
                grid.DisableDots(selectedDots);
                grid.Reorder();
                selectedDots = null;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
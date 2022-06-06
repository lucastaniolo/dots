using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameStateHandler : MonoBehaviour
{
    [SerializeField] private DotGrid grid;

    private IGameState currentState;
    private ColorData colorPicked;
    private RaycastHit2D currentHit;
    private List<DotData> selectedDots = new();
    private List<DotData> squaredDots = new();

    public static event Action<List<DotData>> DotsSelectedEvent;
    public static event Action<ColorData> SquarePreSelectionEvent;
    public static event Action<List<DotData>> ReplenishEvent;
    public static event Action<int, Vector2> SelectionDraggingEvent;

    public static event Action DotUnselectedEvent;
    public static event Action ClearSelectionEvent;

    private void Awake() => Input.multiTouchEnabled = false;

    private void OnEnable() => ChangeState(new IdleState());

    private void Update() => HandleInput();

    #region Global Events Notifications

    private void NotifySquarePreSelection()
    {
        NotifyDotsSelection(squaredDots);
        SquarePreSelectionEvent?.Invoke(colorPicked);
    }

    private void NotifyDotsSelection(List<DotData> data) => DotsSelectedEvent?.Invoke(data);

    private void NotifyDotUnselected() => DotUnselectedEvent?.Invoke();

    private void NotifyReplenishment() => ReplenishEvent?.Invoke(selectedDots);

    private void NotifySelectionCleared() => ClearSelectionEvent?.Invoke();

    #endregion

    #region Selection methods

    public void SelectDot(DotData dotData)
    {
        selectedDots.Add(dotData);
        NotifyDotsSelection(new List<DotData> { dotData });
    }

    // No need to pass an argument, deselect always target the last dot
    public void DeselectDot()
    {
        // List can have duplicates because of Square mechanic
        selectedDots.RemoveAt(selectedDots.Count - 1);
        NotifyDotUnselected();
    }

    public void PreSelectSquaredDots()
    {
        squaredDots = grid.GetDotsByColor(colorPicked);
        NotifySquarePreSelection();
    }

    public void ResolveSelection(bool success)
    {
        if (success)
        {
            NotifyReplenishment();
            grid.DisableDots(squaredDots.Count > 0 ? squaredDots : selectedDots);
            grid.Reorder();
            grid.AnimateDroppingDots();
        }

        NotifySelectionCleared();
        selectedDots.Clear();
        ClearSquaredDots();
    }

    #endregion

    #region State methods

    public void ChangeState(IGameState newState)
    {
        print($"Change state from {currentState?.GetType()} to {newState.GetType()}");
        currentState = newState;
        currentState.UpdateState(this);
    }

    public void PickColor(ColorData colorData) => colorPicked = colorData;

    private bool IsDotSelected(DotData dotData) => selectedDots.Contains(dotData);

    public bool CanDisconnectDot(DotData dotData) => IsDotSelected(dotData) && dotData == selectedDots[^2];

    public int GetSelectionCount() => selectedDots.Count;

    public bool CanConnectDot(DotData dotData)
    {
        return dotData.ColorData.ColorId == colorPicked.ColorId &&
               dotData.GridIndex.IsAdjacent(selectedDots[^1].GridIndex);
    }

    public bool CanCompleteSquare(DotData dotData) => CanConnectDot(dotData) && IsDotSelected(dotData);

    public bool IsSquareActive() => selectedDots.Count != selectedDots.Distinct().Count();
    
    public void ClearSquaredDots() => squaredDots.Clear();

    public void UpdateDragPosition() => SelectionDraggingEvent?.Invoke(GetSelectionCount(), currentHit.point);

    #endregion

    #region Inputs

    private void HandleInput()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        currentHit = Physics2D.Raycast(ray.origin, ray.direction);

        if (Input.GetMouseButtonUp(0))
            currentState.OnRelease();

        if (Input.GetMouseButtonDown(0))
            currentState.OnPress(currentHit);

        if (Input.GetMouseButton(0))
            currentState.OnDrag(currentHit);
    }

    #endregion
}
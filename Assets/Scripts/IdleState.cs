using UnityEngine;

public class IdleState : IGameState
{
    private GameStateHandler stateHandler;

    public void UpdateState(GameStateHandler gameStateHandler)
    {
        stateHandler = gameStateHandler;
        stateHandler.ResolveSelection(stateHandler.GetSelectionCount() > 1);
    }
    
    public void OnPress(RaycastHit2D hit)
    {
        if (hit.transform.TryGetComponent(out Dot dot))
        {
            stateHandler.PickColor(dot.Data.ColorData);
            stateHandler.SelectDot(dot.Data);
            stateHandler.ChangeState(new SingleDotSelectedState());
        }
        else
        {
            stateHandler.ChangeState(new ColorPickingState());
        }
    }

    public void OnDrag(RaycastHit2D hit) { }

    public void OnRelease() { }
}
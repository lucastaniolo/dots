using UnityEngine;

public class ColorPickingState : IGameState
{
    private GameStateHandler stateHandler;
    
    public void UpdateState(GameStateHandler gameStateHandler)
    {
        stateHandler = gameStateHandler;
    }

    public void OnPress(RaycastHit2D hit) { }
    
    public void OnDrag(RaycastHit2D hit)
    {
        if (!hit.transform.TryGetComponent(out Dot dot)) return;

        stateHandler.PickColor(dot.Data.ColorData);
        stateHandler.SelectDot(dot.Data);
        stateHandler.ChangeState(new SingleDotSelectedState());
    }

    public void OnRelease()
    {
        stateHandler.ChangeState(new IdleState());
    }
}
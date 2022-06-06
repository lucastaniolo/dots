using UnityEngine;

public class SingleDotSelectedState : IGameState
{
    private GameStateHandler stateHandler;

    public void UpdateState(GameStateHandler gameStateHandler)
    {
        stateHandler = gameStateHandler;
    }

    public void OnPress(RaycastHit2D hit) { }

    public void OnDrag(RaycastHit2D hit)
    {
        stateHandler.UpdateDragPosition();

        if (!hit.transform.TryGetComponent(out Dot dot)) return;

        if (!stateHandler.CanConnectDot(dot.Data)) return;
        
        stateHandler.SelectDot(dot.Data);
        stateHandler.ChangeState(new ConnectingState());
    }

    public void OnRelease()
    {
        stateHandler.ChangeState(new IdleState());
    }
}
using UnityEngine;

public class ConnectingState : IGameState
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

        var dotData = dot.Data;
        
        if (stateHandler.CanDisconnectDot(dotData))
        {
            stateHandler.DeselectDot();
            
            if (stateHandler.GetSelectionCount() < 2)
                stateHandler.ChangeState(new SingleDotSelectedState());
        }

        if (stateHandler.CanCompleteSquare(dotData))
        {
            stateHandler.SelectDot(dotData);
            stateHandler.ChangeState(new SquaredState());
            return;
        }

        if (stateHandler.CanConnectDot(dotData))
            stateHandler.SelectDot(dotData);
    }

    public void OnRelease()
    {
        stateHandler.ChangeState(new IdleState());
    }
}
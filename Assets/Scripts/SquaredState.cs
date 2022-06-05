using UnityEngine;

public class SquaredState : IGameState
{
    private GameStateHandler stateHandler;
    
    public void UpdateState(GameStateHandler gameStateHandler)
    {
        stateHandler = gameStateHandler;
        stateHandler.PreSelectSquaredDots();
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

            if (!stateHandler.IsSquareActive())
            {
                stateHandler.ClearSquaredDots();
                stateHandler.ChangeState(new ConnectingState());
                return;
            }
        }

        if (stateHandler.CanConnectDot(dotData))
            stateHandler.SelectDot(dotData);
    }

    public void OnRelease()
    {
        stateHandler.ChangeState(new IdleState());
    }
}
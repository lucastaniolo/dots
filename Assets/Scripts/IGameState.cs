using UnityEngine;

public interface IGameState
{
    void UpdateState(GameStateHandler gameStateHandler);
    void OnPress(RaycastHit2D hit);
    void OnDrag(RaycastHit2D hit);
    void OnRelease();
}
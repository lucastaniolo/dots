public class ShrinkEffectPool : DotEffectPool
{
    private void OnEnable() => GameStateHandler.ReplenishEvent += PlaceEffect;
    
    private void OnDisable() => GameStateHandler.ReplenishEvent -= PlaceEffect;
}
public class ShrinkEffectPool : DotEffectPool
{
    private void OnEnable() => GameManager.ReplenishEvent += PlaceEffect;
    
    private void OnDisable() => GameManager.ReplenishEvent -= PlaceEffect;
}
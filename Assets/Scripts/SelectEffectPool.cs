public class SelectEffectPool : DotEffectPool
{
    private void OnEnable() => GameStateHandler.DotsSelectedEvent += PlaceEffect;

    private void OnDisable() => GameStateHandler.DotsSelectedEvent -= PlaceEffect;
}
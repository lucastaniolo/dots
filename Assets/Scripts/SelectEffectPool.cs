public class SelectEffectPool : DotEffectPool
{
    private void OnEnable() => GameManager.DotsSelectedEvent += PlaceEffect;

    private void OnDisable() => GameManager.DotsSelectedEvent -= PlaceEffect;
}
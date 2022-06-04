using UnityEngine;

public class SelectionEffect : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void SetupView(DotData data)
    {
        spriteRenderer.color = data.ColorData.Color;
    }
}

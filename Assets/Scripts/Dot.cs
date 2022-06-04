using System;
using UnityEngine;

public class Dot : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public static event Action<Dot> DisableEvent;
    
    public DotData Data { get; private set; }

    public bool IsActive => spriteRenderer.enabled;
    
    public void SetupData(DotData data)
    {
        Data = data;
        spriteRenderer.color = Data.ColorData.Color;
    }
    
    public void MoveTo(Vector2 position)
    {
        transform.localPosition = position;
    }

    public void SnapToGridPosition()
    {
        transform.localPosition = Data.GridPosition;
    }

    public void Disable()
    {
        DisableEvent?.Invoke(this);
        spriteRenderer.enabled = false;
    }

    public void Reuse()
    {
        Data.ColorData = GamePalette.Instance.GetRandomColor();
        spriteRenderer.color = Data.ColorData.Color;
        spriteRenderer.enabled = true;
    }

    public void OnDrag()
    {
        if (IsActive)
            Disable();
    }
}

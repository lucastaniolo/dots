using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Dot : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public DotData Data { get; private set; }

    public bool IsActive => spriteRenderer.enabled;

    private bool reusing;

    private readonly Dictionary<int, float> delayByGridY = new()
    {
        { 0, 0.18f },
        { 1, 0.14f },
        { 2, 0.1f },
        { 3, 0.06f },
        { 4, 0.02f },
        { 5, 0f }
    };

    public void SetupData(DotData data)
    {
        Data = data;
        spriteRenderer.color = Data.ColorData.Color;
        transform.localPosition = Data.GridPosition + Vector2.up * 15;
    }
    
    public void MoveTo()
    {
        transform.DOLocalMoveY(Data.GridPosition.y, 0.5f)
            .SetEase(Ease.OutBounce)
            .SetDelay(delayByGridY[Data.GridIndex.y])
            .OnComplete(SnapToGridPosition);

        reusing = false;
    }

    public void SnapToGridPosition()
    {
        transform.localPosition = Data.GridPosition;
    }

    public void Disable()
    {
        spriteRenderer.enabled = false;
    }

    public void Reuse()
    {
        Data.ColorData = GamePalette.Instance.GetRandomColor();
        spriteRenderer.color = Data.ColorData.Color;
        spriteRenderer.enabled = true;

        transform.localPosition = Data.GridPosition + Vector2.up * Camera.main.orthographicSize;
        reusing = true;
    }
}

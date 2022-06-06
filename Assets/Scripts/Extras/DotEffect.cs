using System;
using System.Collections;
using UnityEngine;

public class DotEffect : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;

    private static WaitForSeconds waitAnimation;
    
    public void SetupView(DotData data, Action animationFinishCallback)
    {
        spriteRenderer.color = data.ColorData.Color;
        transform.position = data.GridPosition;

        StartCoroutine(InvokeCallback());
        IEnumerator InvokeCallback()
        {
            yield return waitAnimation ??= new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            animationFinishCallback?.Invoke();
        }
    }
}

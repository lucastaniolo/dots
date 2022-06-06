using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class DotEffectPool : MonoBehaviour
{
    [SerializeField] private DotEffect selectionFX;

    private IObjectPool<DotEffect> pool;

    private void Start()
    {
        pool = new ObjectPool<DotEffect>(
            () => Instantiate(selectionFX, transform),
            fx => { fx.gameObject.SetActive(true); },
            fx => { fx.gameObject.SetActive(false); },
            fx => { Destroy(fx.gameObject); },
            false);
    }
    
    protected void PlaceEffect(IEnumerable<DotData> data)
    {
        foreach (var d in data)
        {
            var fx = pool.Get();
            fx.SetupView(d, () => pool.Release(fx));
        }
    }
}
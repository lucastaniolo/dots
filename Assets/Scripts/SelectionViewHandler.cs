using UnityEngine;
using UnityEngine.Pool;

public class SelectionViewHandler : MonoBehaviour
{
    [SerializeField] private SelectionEffect selectionFX;

    private IObjectPool<SelectionEffect> pool;

    private void Start()
    {
        pool = new ObjectPool<SelectionEffect>(
            () => Instantiate(selectionFX, transform),
            fx => { fx.gameObject.SetActive(true); },
            fx => { fx.gameObject.SetActive(false); },
            fx => { Destroy(fx.gameObject); },
            false);
    }

    private void OnEnable()
    {
        GameManager.DotSelectedEvent += PlayEffect;
    }

    private void OnDisable()
    {
        GameManager.DotSelectedEvent -= PlayEffect;
    }

    private void PlayEffect(DotData data)
    {
        var fx = pool.Get();
        fx.SetupView(data, () => pool.Release(fx));
    }
}
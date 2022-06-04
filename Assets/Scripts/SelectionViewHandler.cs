using UnityEngine;

public class SelectionViewHandler : MonoBehaviour
{
    [SerializeField] private SelectionEffect selectionFX;

    private void OnEnable()
    {
        GameManager.DotSelectedEvent += CreateEffect;
    }

    private void OnDisable()
    {
        GameManager.DotSelectedEvent -= CreateEffect;
    }

    private void CreateEffect(DotData data)
    {
        Instantiate(selectionFX, data.GridPosition, Quaternion.identity).SetupView(data);
    }
}
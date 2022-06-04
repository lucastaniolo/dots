using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraBackground : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Camera>().backgroundColor = GamePalette.Instance.Data.Background;
    }
}

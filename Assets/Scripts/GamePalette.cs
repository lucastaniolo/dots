using UnityEngine;

public class GamePalette : MonoBehaviour
{
    [field: SerializeField] public PaletteData Data { get; private set; }

    public static GamePalette Instance;
    
    private void Awake() => Instance = this;

    public ColorData GetRandomColor() => Data.Dots.GetRandom();
}
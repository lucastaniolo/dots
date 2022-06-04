using UnityEngine;

[CreateAssetMenu(fileName = "ColorPalette")]
public class PaletteData : ScriptableObject
{
    [field: SerializeField] public ColorData[] Dots { get; private set; }
    [field: SerializeField] public Color Background { get; private set; }
}
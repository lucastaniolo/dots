using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SquarePanel : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private Image fullBorder;
        
    [SerializeField] private Slider[] horizontalLines;
    [SerializeField] private Slider[] verticalLines;

    [SerializeField] private Slider[] allLines;

    [SerializeField] private int stepsToFill = 8;

    private void OnEnable()
    {
        SetupSliders();
        Reset();
        
        GameStateHandler.DotsSelectedEvent += Grow;
        GameStateHandler.SquarePreSelectionEvent += ActivateSquare;
        GameStateHandler.DotUnselectedEvent += Shrink;
        GameStateHandler.ClearSelectionEvent += Reset;
        GameStateHandler.SquareCanceled += CancelSquare;
    }

    private void OnDisable()
    {
        GameStateHandler.DotsSelectedEvent -= Grow;
        GameStateHandler.SquarePreSelectionEvent -= ActivateSquare;
        GameStateHandler.DotUnselectedEvent -= Shrink;
        GameStateHandler.ClearSelectionEvent -= Reset;
        GameStateHandler.SquareCanceled -= CancelSquare;
    }

    private void SetupSliders()
    {
        foreach (var l in allLines)
            l.maxValue = stepsToFill;
    }

    private void Shrink()
    {
        var lines = verticalLines[0].value > 0 ? verticalLines : horizontalLines;
        foreach (var l in lines)
            l.SetValueWithoutNotify(--l.value);
    }
    
    private void Grow(List<DotData> dotData)
    {
        if (horizontalLines[0].value == 0)
        {
            foreach (var l in allLines)
                l.targetGraphic.color = dotData[0].ColorData.Color;
        }

        var lines = horizontalLines[0].value < stepsToFill ? horizontalLines : verticalLines;
        foreach (var l in lines)
            l.SetValueWithoutNotify(++l.value);
    }

    private void ActivateSquare(ColorData colorData)
    {
        background.color = colorData.Color;
        fullBorder.color = colorData.Color;
        
        background.enabled = true;
        fullBorder.enabled = true;
    }
    
    private void CancelSquare()
    {
        Shrink();
        
        background.enabled = false;
        fullBorder.enabled = false;
    }

    private void Reset()
    {
        background.enabled = false;
        fullBorder.enabled = false;

        foreach (var l in allLines)
            l.SetValueWithoutNotify(0);
    }
}

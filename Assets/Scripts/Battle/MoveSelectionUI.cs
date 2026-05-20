using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;

public class MoveSelectionUI : MonoBehaviour
{
    [SerializeField] List<Text> moveTexts;
    Color highlightedColor;

    int currentSelection = 0;


    private void Start()
    {
        highlightedColor = GlobalSettings.i.HighlightedColor;
    }
    public void SetMoveData(List<MoveBase> currentMoves, MoveBase newMove)
    {
        for (int i=0; i<currentMoves.Count; i++)
        {
            moveTexts[i].text = currentMoves[i].Name;
        }

        moveTexts[currentMoves.Count].text = newMove.Name;
    }

    public void HandleMoveSelection(Action<int> onSelected)
    {
        if (Keyboard.current.downArrowKey.wasPressedThisFrame || Keyboard.current.sKey.wasPressedThisFrame)
            ++currentSelection;
        else if (Keyboard.current.upArrowKey.wasPressedThisFrame || Keyboard.current.wKey.wasPressedThisFrame)
            --currentSelection;

        currentSelection = Mathf.Clamp(currentSelection, 0, PokemonBase.MaxNumOfMoves);

        UpdateMoveSelection(currentSelection);

        if (Keyboard.current.zKey.wasPressedThisFrame)
            onSelected?.Invoke(currentSelection);
    }

    public void UpdateMoveSelection(int selection)
    {
        for (int i = 0; i < PokemonBase.MaxNumOfMoves+1; i++)
        {
            if (i == selection)
                moveTexts[i].color = highlightedColor;
            else
                moveTexts[i].color = Color.black;
        }
    }
}

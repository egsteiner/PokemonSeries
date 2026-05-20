using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject menu;

    public event Action<int> onMenuSelected;
    public event Action onBack;


    List<Text> menuItems;

    int selectedItem = 0;

    private void Awake()
    {
        menuItems = menu.GetComponentsInChildren<Text>().ToList();
    }
    public void OpenMenu()
    {
        menu.SetActive(true);
        UpdateItemSelection();
    }

    public void CloseMenu()
    {
        menu.SetActive(false);
    }



    public void HandleUpdate()
    {
        int prevSelection = selectedItem;

        if (Keyboard.current.downArrowKey.wasPressedThisFrame)
            ++selectedItem;

        else if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            --selectedItem;

        selectedItem = Mathf.Clamp(selectedItem, 0, menuItems.Count - 1);

        if (prevSelection != selectedItem)
            UpdateItemSelection();

        if (Keyboard.current.zKey.wasPressedThisFrame)
        {
            onMenuSelected?.Invoke(selectedItem);
            CloseMenu();
        }
        else if (Keyboard.current.xKey.wasPressedThisFrame)
        {
            onBack?.Invoke();
            CloseMenu();
        }
    }

    void UpdateItemSelection()
    {
        for (int i = 0; i < menuItems.Count; ++i)
        {
            if (i == selectedItem)
                menuItems[i].color = GlobalSettings.i.HighlightedColor;
            else
                menuItems[i].color = Color.black;
        }
    }
}


using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//Script for the party screen
public class PartyScreen : MonoBehaviour
{
    [SerializeField] Text messageText;

    PartyMemberUI[] memberSlots;
    List<Pokemon> pokemons;
    PokemonParty party;

    int selection = 0;

    public Pokemon SelectedMember => pokemons[selection];

    // party screen can be called from different states like ActionSelection, RunningTurn, AboutToUse
    public BattleState? CalledFrom { get; set; }

    //Get the number of Pokemon in party
    public void Init()
    {
        memberSlots = GetComponentsInChildren<PartyMemberUI>(true);

        party = PokemonParty.GetPlayerParty();
        SetPartyData();

        party.OnUpdated += SetPartyData;
    }

    //Show Pokemon info for number of members (Ex: if 5 pokemon in party, only show 5 slots
    public void SetPartyData()
    {
        pokemons = party.Pokemons;

        for (int i = 0; i < memberSlots.Length; i++)
        {
            if (i < pokemons.Count)
            {
                memberSlots[i].gameObject.SetActive(true);
                memberSlots[i].Init(pokemons[i]);
            }
            else
                memberSlots[i].gameObject.SetActive(false);
        }

        UpdateMemberSelection(selection);

        messageText.text = "Choose a Pokemon";
    }

    public void HandleUpdate(Action onSelected, Action onBack)
    {
        var prevSelection = selection;

        //Changing highlighted move based on arrows/WASD
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame || Keyboard.current.dKey.wasPressedThisFrame)
            ++selection;
        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame || Keyboard.current.aKey.wasPressedThisFrame)
            --selection;
        else if (Keyboard.current.downArrowKey.wasPressedThisFrame || Keyboard.current.sKey.wasPressedThisFrame)
            selection += 2;
        else if (Keyboard.current.upArrowKey.wasPressedThisFrame || Keyboard.current.wKey.wasPressedThisFrame)
            selection -= 2;

        selection = Mathf.Clamp(selection, 0, pokemons.Count - 1);

        if (selection != prevSelection)
            UpdateMemberSelection(selection);

        //If z pressed, try to switch the Pokemon, unless it's fainted, or the Pokemon currently in battle

            // if x key is preseed, back out of party screen go back to battle screen
        if (Keyboard.current.zKey.wasPressedThisFrame)
        {
            onSelected?.Invoke();
        }
        else if (Keyboard.current.xKey.wasPressedThisFrame)
        {
            onBack?.Invoke();
        }
    }

    //Updating the Pokemon the user is highlighting/selected in the party screen
    public void UpdateMemberSelection(int selectedMember)
    {
        for (int i = 0; i < pokemons.Count; i++)
        {
            if (i == selectedMember)
                memberSlots[i].SetSelected(true);
            else
                memberSlots[i].SetSelected(false);
        }
    }


    public  void ShowIfTmIsUsable(TmItem tmItem)
    {
        for (int i = 0; i < pokemons.Count; i++)
        {
            string message = tmItem.CanBeTaught(pokemons[i]) ? "ABLE" : "NOT ABLE";
            memberSlots[i].SetMessage(message);
        }
    }

    public void ClearMemberSlotMessages()
    {
        for (int i = 0; i < pokemons.Count; i++)
        {
            memberSlots[i].SetMessage("message");
        }
    }

    //Set the message at the bottom of party screen
    public void SetMessageText(string message)
    {
        messageText.text = message;
        
    }
}


using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO.IsolatedStorage;

//Script for the party screen
public class PartyScreen : MonoBehaviour
{
    [SerializeField] Text messageText;

    PartyMemberUI[] memberSlots;
    List<Pokemon> pokemons;

    //Get the number of Pokemon in party
    public void Init()
    {
        memberSlots = GetComponentsInChildren<PartyMemberUI>();
    }

    //Show Pokemon info for number of members (Ex: if 5 pokemon in party, only show 5 slots
    public void SetPartyData(List<Pokemon> pokemons)
    {
        this.pokemons = pokemons;

        for (int i = 0; i < memberSlots.Length; i++)
        {
            if (i < pokemons.Count)
                memberSlots[i].SetData(pokemons[i]);
            else
                memberSlots[i].gameObject.SetActive(false);
        }

        messageText.text = "Choose a Pokemon";
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

    //Set the message at the bottom of party screen
    public void SetMessageText(string message)
    {
        messageText.text = message;
    }
}

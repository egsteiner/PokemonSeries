using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//Get data for each member of party to display in party screen
public class PartyMemberUI : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;
    [SerializeField] Text messageText;


    Pokemon _pokemon;


    
    //Get the name level and hp of the pokemon, set them to the HUD values in the battle screen
    public void Init(Pokemon pokemon)
    {
        _pokemon = pokemon;
        UpdateData();

        _pokemon.OnHPChanged += UpdateData;
        SetMessage("");
    }

    void UpdateData()
    {
        nameText.text = _pokemon.Base.Name;
        levelText.text = "Lvl " + _pokemon.Level;
        hpBar.SetHP((float)_pokemon.HP / _pokemon.MaxHp);
    }

    //Updates HP Bar after an attack
    public IEnumerator UpdateHP()
    {
        yield return hpBar.SetHPSmooth((float)_pokemon.HP / _pokemon.MaxHp);
    }

    //Highlight the name of the selected pokemon in the party
    public void SetSelected(bool selected)
    {
        if (selected)
            nameText.color = GlobalSettings.i.HighlightedColor;
        else
            nameText.color = Color.black;
    }

    public void SetMessage(string message)
    {
        messageText.text = message;
    }
}

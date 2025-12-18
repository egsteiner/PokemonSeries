using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] PokemonBase _base;
    [SerializeField] int level;
    [SerializeField] bool isPlayerUnit;

    public Pokemon Pokemon { get; set; }
    
    //Make new Pokemon instance using the provided pokemon, get back sprite if player's, front sprite if enemy's
    public void Setup()
    {
        Pokemon = new Pokemon(_base, level);
        if (isPlayerUnit)
            GetComponent<Image>().sprite = Pokemon.Base.BackSprite;
        else GetComponent<Image>().sprite = Pokemon.Base.FrontSprite;
    }

}

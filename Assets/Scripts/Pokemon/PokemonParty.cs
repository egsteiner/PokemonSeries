using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

//Script that has list of all Pokemon in user's party
public class PokemonParty : MonoBehaviour
{
    [SerializeField] List<Pokemon> pokemons;

    public List<Pokemon> Pokemons
    {
        get
        {
            return pokemons;
        }
    }

    //Initialize each Pokemon
    private void Start()
    {
        foreach (var pokemon in pokemons)
        {
            pokemon.Init();
        }
    }

    //Return list of all Pokemon that aren't fainted
    public Pokemon GetHealthyPokemon()
    {
        return pokemons.Where(x => x.HP > 0).FirstOrDefault();
    }
}

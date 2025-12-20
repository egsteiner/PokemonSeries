using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

//Script to get wild Pokemon from list of possible encounters
public class MapArea : MonoBehaviour
{
    [SerializeField] List<Pokemon> wildPokemons;

    public Pokemon GetRandomWildPokemon()
    {
        var wildPokemon = wildPokemons[Random.Range(0, wildPokemons.Count)];
        wildPokemon.Init();
        return wildPokemon;
    }
}

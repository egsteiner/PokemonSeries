using System.Collections.Generic;
using UnityEngine;

//An instance of a Pokemon
public class Pokemon
{
    //Instance of a pokemon has a base generic type that all Pokemon have, and current level, current HP, and the Moves the Pokemon currently knows
    public PokemonBase Base { get; set; }
    public int Level { get; set; }

    //Quick way of creating property, used when we don't need it displayed in the Unity Inspector
    public int HP { get; set; }

    public List<Move>  Moves { get; set; }

    //Constructor that sets the Pokemon level, base information/stats and sets current HP to MaxHp
    public Pokemon(PokemonBase pBase, int pLevel)
    {
        Base = pBase;
        Level = pLevel;
        HP = MaxHp;

        //Makes list of the moves the pokemon currently has. Goes through learnable moves of Pokemon by level, adds the first four, then stops
        Moves = new List<Move>();
        foreach (var move in Base.LearnableMoves)
        {
            if (move.Level <= Level)
                Moves.Add(new Move(move.Base));

            if (Moves.Count >= 4)
                break;
        }
    }

    //Uses official Pokemon's calculations to determine Pokemon's stats at it's current level based on base stats
    public int Attack
    {
        get { return Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5; }
    }

    public int Defense
    {
        get { return Mathf.FloorToInt((Base.Defense * Level) / 100f) + 5; }
    }

    public int SpAttack
    {
        get { return Mathf.FloorToInt((Base.SpAttack * Level) / 100f) + 5; }
    }

    public int SpDefense
    {
        get { return Mathf.FloorToInt((Base.SpDefense * Level) / 100f) + 5; }
    }

    public int MaxHp
    {
        get { return Mathf.FloorToInt((Base.MaxHp * Level) / 100f) + 10; }
    }

    public int Speed
    {
        get { return Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5; }
    }
}

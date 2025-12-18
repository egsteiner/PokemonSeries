using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Pokemon", menuName = "Pokemon/Create new pokemon")]


//Generic description of what EVERY pokemon has
public class PokemonBase : ScriptableObject
{

    //They all have name, description, sprites, types, stats, and a list of learnable moves
    [SerializeField] string name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] Sprite frontSprite;
    [SerializeField] Sprite backSprite;

    [SerializeField] PokemonType type1;
    [SerializeField] PokemonType type2;

    [SerializeField] int maxHp;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int spAttack;
    [SerializeField] int spDefense;
    [SerializeField] int speed;

    //A list of LearnableMoves
    [SerializeField] List<LearnableMove> learnableMoves;

    //Properties, essentially getters for C#
    //Have for all the variables above
    public string Name
    {
        get { return name; }
    }

    public string Description
    {
        get { return description; }
    }

    public Sprite FrontSprite
    {
        get { return frontSprite; }
    }

    public Sprite BackSprite
    {
        get { return backSprite; }
    }

    public PokemonType Type1
    {
        get { return type1; }
    }

    public PokemonType Type2
    {
        get { return type2; }
    }

    public int MaxHp
    {
        get { return maxHp; }
    }

    public int Attack
    {
        get { return attack; }
    }

    public int Defense
    {
        get { return defense; }
    }

    public int SpAttack
    {
        get { return spAttack; }
    }

    public int SpDefense
    {
        get { return spDefense; }
    }

    public int Speed
    {
        get { return speed; }
    }

    public List<LearnableMove> LearnableMoves
    {
        get { return learnableMoves; }
    }

}

//A Learnable Move is  a Move, thus has a generic MoveBase attribute, as well as the level the move is learned based on the pokemon
[System.Serializable]
public class LearnableMove
{
    [SerializeField] MoveBase moveBase;
    [SerializeField] int level;

    //Properties for both
    public MoveBase Base
    {
        get { return moveBase; }
    }

    public int Level
    {
        get { return level; }
    }

}

//Enums are groups of CONSTANTS. 
//Here it is group of all pokemon types. NONE is for single type pokemon, second type is none
public enum PokemonType
{
    None,
    Normal,
    Fire,
    Water,
    Electric,
    Grass,
    Ice,
    Fighting,
    Poison,
    Ground,
    Flying,
    Psychic,
    Bug,
    Rock,
    Ghost,
    Dragon
}
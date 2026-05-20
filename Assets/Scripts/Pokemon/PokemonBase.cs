using UnityEngine;
using System.Collections.Generic;
using System;

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

    [SerializeField] int expYield;
    [SerializeField] GrowthRate growthRate;

    [SerializeField] int catchRate = 255;

    //A list of LearnableMoves
    [SerializeField] List<LearnableMove> learnableMoves;
    [SerializeField] List<MoveBase> learnableByItems;

    public static int MaxNumOfMoves { get; set; } = 4;

    public int GetExpForLevel(int level)
    {
        if (growthRate == GrowthRate.Fast)
        {
            
            return 4 * (level * level * level) / 5;
            
        }

        else if (growthRate == GrowthRate.MediumFast)
        {
            
            return level * level * level;
        }

        else if (growthRate == GrowthRate.MediumSlow)
        {
            
            return (6 * (level * level * level) / 5) - (15 * (level * level)) + (100 * level) - 140;
            
        }

        else if (growthRate == GrowthRate.Slow)
        {
            
            return 5 * (level * level * level) / 4;
        }

        else if (growthRate == GrowthRate.Erratic)
        {
            if (level < 50)
                return ((level * level * level) * (100 - level)) / 50;
            else if (level >= 50 && level < 68)
                return ((level * level * level) * (150 - level)) / 100;
            else if (level >= 68 && level < 98)
                return ((level * level * level) * (Mathf.FloorToInt((1911f - (10 * level)) / 3))) / 500;
            else if (level >= 98 && level <= 100)
                return ((level * level * level) * (160 - level)) / 100;

        }

        else if (growthRate == GrowthRate.Fluctuating)
        {
            if (level < 15)
                return ((level * level * level) * (Mathf.FloorToInt((level + 1) / 3) + 24)) / 50;
            else if (level <= 15 && level < 36)
                return ((level * level * level) * (level + 14)) / 50;
            else if (level <= 36 && level <= 100)
                return ((level * level * level) * (Mathf.FloorToInt(level / 2) + 32)) / 50;
        }

        return -1;
    }

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

    public List<MoveBase> LearnableByItems => learnableByItems;

    public int CatchRate => catchRate;

    public int ExpYield => expYield;

    public GrowthRate GrowthRate => growthRate;

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
    Dragon,
    Dark,
    Steel
}

public enum GrowthRate
{
    Fast, MediumFast, Erratic, MediumSlow, Slow, Fluctuating
}

public enum Stat
{
    Attack,
    Defense,
    SpAttack,
    SpDefense,
    Speed,

    // These 2 not actual stats, used for boost to moveAccuracy
    Accuracy,
    Evasion
}

//Type chart of all type effectiveness
public class TypeChart
{
    static float[][] chart =
    {
        //                    NOR  FIR     WAT     ELE   GRA   ICE   FIG   POI   GRO   FLY   PSY   BUG   ROC   GHO   DRA   DAR   STE
        /*NOR*/ new float [] { 1f, 1f,     1f,     1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   0.5f, 0f,   1f,   1f,   0.5f },
        /*FIR*/ new float [] { 1f, 0.5f,   0.5f,   1f,   2f,   2f,   1f,   1f,   1f,   1f,   1f,   2f,   0.5f, 1f,   0.5f, 1f,   2f },
        /*WAT*/ new float [] { 1f, 2f,     0.5f,   1f,   0.5f, 1f,   1f,   1f,   2f,   1f,   1f,   1f,   2f,   1f,   0.5f, 1f,   1f },
        /*ELE*/ new float [] { 1f, 1f,     2f,     0.5f, 0.5f, 1f,   1f,   1f,   0f,   2f,   1f,   1f,   1f,   1f,   0.5f, 1f,   1f},
        /*GRA*/ new float [] { 1f, 0.5f,   2f,     1f,   0.5f, 1f,   1f,   0.5f, 2f,   0.5f, 1f,   0.5f, 2f,   1f,   0.5f, 1f,   0.5f},
        /*ICE*/ new float [] { 1f, 0.5f,   0.5f,   1f,   2f,   0.5f, 1f,   1f,   2f,   2f,   1f,   1f,   1f,   1f,   2f,   1f,   0.5f},
        /*FIG*/ new float [] { 2f, 1f,     1f,     1f,   1f,   2f,   1f,   0.5f, 1f,   0.5f, 0.5f, 0.5f, 2f,   0f,   1f,   2f,   2f },
        /*POI*/ new float [] { 1f, 1f,     1f,     1f,   2f,   1f,   1f,   0.5f, 0.5f, 1f,   1f,   1f,   0.5f, 0.5f, 1f,   1f,   0f  },
        /*GRO*/ new float [] { 1f, 2f,     1f,     2f,   0.5f, 1f,   1f,   2f,   1f,   0f,   1f,   0.5f, 2f,   1f,   1f,   1f,   2f  },
        /*FLY*/ new float [] { 1f, 1f,     1f,     0.5f, 2f,   1f,   2f,   1f,   1f,   1f,   1f,   2f,   0.5f, 1f,   1f,   1f,   0.5f  },
        /*PSY*/ new float [] { 1f, 1f,     1f,     1f,   1f,   1f,   2f,   2f,   1f,   1f,   0.5f, 1f,   1f,   1f,   1f,   0f,   0.5f  },
        /*BUG*/ new float [] { 1f, 0.5f,   1f,     1f,   2f,   1f,   0.5f, 0.5f, 1f,   0.5f, 2f,   1f,   1f,   0.5f, 1f,   2f,   0.5f  },
        /*ROC*/ new float [] { 1f, 2f,     1f,     1f,   1f,   2f,   0.5f, 1f,   0.5f, 2f,   1f,   2f,   1f,   1f,   1f,   1f,   0.5f  },
        /*GHO*/ new float [] { 0f, 1f,     1f,     1f,   1f,   1f,   1f,   1f,   1f,   1f,   2f,   1f,   1f,   2f,   1f,   0.5f, 1f  },
        /*DRA*/ new float [] { 1f, 1f,     1f,     1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   2f,   1f,   0.5f  },
        /*DAR*/ new float [] { 1f, 1f,     1f,     1f,   1f,   1f,   0.5f, 1f,   1f,   1f,   2f,   1f,   1f,   2f,   1f,   0.5f, 1f  },
        /*STE*/ new float [] { 1f, 0.5f,   0.5f,   0.5f, 1f,   2f,   1f,   1f,   1f,   1f,   1f,   1f,   2f,   1f,   1f,   1f,   0.5f  },
    };

    //Return the effectiveness of the move on the Pokemon, can be 0, 0.25,  0.5, 1, 2, 4
    public static float GetEffectiveness(PokemonType attackType, PokemonType defenseType)
    {
        if (attackType == PokemonType.None || defenseType == PokemonType.None) return 1;

        int row = (int)attackType - 1;
        int col = (int)defenseType - 1;

        return chart[row][col];
    }
}
using System;
using UnityEngine;

//Name, Description, Start Message of each condition
public class Condition
{
    public ConditionID Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; } 
    public string StartMessage { get; set; }

    // Actions for checking Status stuff Start of turn(How many turns of sleep left, before inflicted Pokemon moves(paralyze,freeze,sleep), and after inflicted  Pokemon moves (burn/Poison)
    public Action<Pokemon> OnStart { get; set; }

    public Func<Pokemon, bool> OnBeforeMove { get; set; }
    public Action<Pokemon> OnAfterTurn { get; set; }
}

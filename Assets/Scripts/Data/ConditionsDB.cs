using System.Collections.Generic;
using UnityEngine;

public class ConditionsDB
{
    public static void Init()
    {
        foreach (var kvp in Conditions)
        {
            var conditionId = kvp.Key;
            var condition = kvp.Value;

            condition.Id = conditionId;
        }
    }

    //Dictonary with each status condition, what it do, name, etc
    public static Dictionary<ConditionID, Condition> Conditions { get; set; } = new Dictionary<ConditionID, Condition>()
    {
        {
            ConditionID.psn,
            new Condition()
            {
                Name = "Poison",
                StartMessage = "has been poisoned!",
                OnAfterTurn = (Pokemon pokemon) =>
                {
                    pokemon.DecreaseHP(pokemon.MaxHp / 8);
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is hurt by poison!");
                }
            }
        },
        {
            ConditionID.brn,
            new Condition()
            {
                Name = "Burn",
                StartMessage = "has been burned!",
                OnAfterTurn = (Pokemon pokemon) =>
                {
                    pokemon.DecreaseHP(pokemon.MaxHp / 16);
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is hurt by its burn!");
                }
            }
        },
        {
            ConditionID.par,
            new Condition()
            {
                Name = "Paralyzed",
                StartMessage = "is paralyzed, it may be unable to move!",
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if (Random.Range(1,5) == 1)
                    {
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is paralyzed! It can't move!");
                        return false;
                    }

                    return true;
                }

            }
        },
        {
            ConditionID.frz,
            new Condition()
            {
                Name = "Freeze",
                StartMessage = "became frozen!",
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if (Random.Range(1,5) == 1)
                    {
                        pokemon.CureStatus();
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} thawed out!");
                        return true;
                    }
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is frozen solid!");
                    return false;
                }

            }
        },
        {
            ConditionID.slp,
            new Condition()
            {
                Name = "Sleep",
                StartMessage = "fell asleep!",
                OnStart = (Pokemon pokemon) =>
                {
                    // Sleep for 1-3 turns
                    pokemon.StatusTime = Random.Range(1,4);
                    Debug.Log($"Will sleep for {pokemon.StatusTime} moves");
                },
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if (pokemon.StatusTime <= 0)
                    {
                        pokemon.CureStatus();
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} woke up!");
                        return true;

                    }

                    pokemon.StatusTime--;
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is fast asleep!");
                    return false;
                }

            }
        },

        // Volatile Status Conditions
        {
            ConditionID.confusion,
            new Condition()
            {
                Name = "Confusion",
                StartMessage = "has been confused!",
                OnStart = (Pokemon pokemon) =>
                {
                    // Confused for 1-4 turns
                    pokemon.VolatileStatusTime = Random.Range(1,5);
                    Debug.Log($"Will be confused for {pokemon.VolatileStatusTime} moves");
                },
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if (pokemon.VolatileStatusTime <= 0)
                    {
                        pokemon.CureVolatileStatus();
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} snapped out of confusion!");
                        return true;

                    }

                    pokemon.VolatileStatusTime--;

                    //50% chance to do move
                    if (Random.Range(1, 3) == 1)
                        return true;

                    //Hurt by confusion
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is confused!");
                    pokemon.DecreaseHP(pokemon.MaxHp / 8);
                    pokemon.StatusChanges.Enqueue($"It hurt itself in its confusion!");
                    return false;
                }

            }
        }
    };

    public static float GetStatusBonus(Condition condition)
    {
        if (condition == null)
            return 1f;
        else if (condition.Id == ConditionID.slp || condition.Id == ConditionID.frz)
            return 2f;
        else if (condition.Id == ConditionID.par || condition.Id == ConditionID.psn || condition.Id == ConditionID.brn)
            return 1.5f;

        return 1f;
    }

}

public enum ConditionID
{
    none, psn, brn, slp, par, frz, confusion
}

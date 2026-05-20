using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(menuName = "Items/Create new TM or HM")]
public class TmItem : ItemBase
{
    [SerializeField] MoveBase move;
    [SerializeField] bool isHM;

    public override string Name => base.Name + $": {move.Name}";
    public override bool Use(Pokemon pokemon)
    {
        // Learning move is handled from inventory UI, if it was learned return true
        return pokemon.HasMove(move);
    }

    public bool CanBeTaught(Pokemon pokemon)
    {
        return pokemon.Base.LearnableByItems.Contains(move);
    }

    public override bool IsReusable => isHM;

    

    public override bool CanUseInBattle => false;
    public MoveBase Move => move;
    public bool IsHM => isHM;
}

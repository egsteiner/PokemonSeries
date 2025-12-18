using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "Pokemon/Create new move")]

//Every Move in Pokemon has Generic attributes about it
public class MoveBase : ScriptableObject
{
    //Each Move has a name, description, type, power, accuracy, pp

    [SerializeField] string name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] PokemonType type;
    [SerializeField] int power;
    [SerializeField] int accuracy;
    [SerializeField] int pp;

    //Properties for above attributes
    public string Name
    {
        get { return name; }
    }
    public string Description
    {
        get { return description; }
    }

    public PokemonType Type
    {
        get { return type; }
    }
    public int Power
    {
        get { return power; }
    }
    public int Accuracy
    {
        get { return accuracy; }
    }
    public int PP
    {
        get { return pp; }
    }
}

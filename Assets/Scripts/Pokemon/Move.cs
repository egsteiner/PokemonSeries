using UnityEngine;

//Instance of a Pokemon's  move
public class Move
{
    //Instance of Move has the generic attributes, and the current PP of the move
    public MoveBase Base { get; set; }
    public int PP { get; set; }

    //Constructor that builds the instance of move with the Base Move attributes, sets current PP to base PP
    public Move(MoveBase pBase)
    {
        Base = pBase;
        PP = pBase.PP;
    }
}

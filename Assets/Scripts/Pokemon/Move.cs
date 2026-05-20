using System;
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

    public Move(MoveSaveData saveData)
    {
        Base = MoveDB.GetObjectByName(saveData.name);
        PP = saveData.pp;
    }

    public MoveSaveData GetSaveData()
    {
        var saveData = new MoveSaveData()
        {
            name = Base.name,
            pp = PP
        };

        return saveData;
    }

    public void IncreasePP(int amount)
    {
        PP = Mathf.Clamp(PP + amount, 0, Base.PP);
    }
}

[Serializable]
public class MoveSaveData
{
    public string name;
    public int pp;
}
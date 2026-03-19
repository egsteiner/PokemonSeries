
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialogBox : MonoBehaviour
{
    [SerializeField] Text dialogText;
    [SerializeField] int lettersPerSecond;

    [SerializeField] GameObject actionSelector;
    [SerializeField] GameObject moveSelector;
    [SerializeField] GameObject moveDetails;

    [SerializeField] List<Text> actionTexts;
    [SerializeField] List<Text> moveTexts;

    [SerializeField] Text ppText;
    [SerializeField] Text typeText;

    [SerializeField] Color highlightedColor;

    Coroutine typingCoroutine;



    //Set the dialog box's text to the default text
    public void SetDialog(string dialog)
    {
        dialogText.text = dialog;
    }

    //Make sure only one dialog being typed at a time
    public IEnumerator TypeDialog(string dialog)
    {

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(dialog));

        yield return typingCoroutine;
        
    }

    // Type out the dialog line by line
    private IEnumerator TypeText(string dialog)
    {
        dialogText.text = "";
        foreach (var letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }

        yield return new WaitForSeconds(1f);
        typingCoroutine = null;
    }

    //Enable/show the Dialog Text(Encounter text)
    public void EnableDialogText(bool enabled)
    {
        dialogText.enabled = enabled;
    }

    //Enable/show the Action Selector (Fight/Run) text
    public void EnableActionSelector(bool enabled)
    {
        actionSelector.SetActive(enabled);
    }

    //Enable/show the Move Selector text
    public void EnableMoveSelector(bool enabled)
    {
        moveSelector.SetActive(enabled);
        moveDetails.SetActive(enabled);
    }

    //Highlight which action we choosing
    public void UpdateActionSelection(int selectedAction)
    {
        //check each of the actions to see if its the one we highlighting
        for (int i = 0; i < actionTexts.Count; ++i)
        {
            //once we find it, highlight it, otherwise keep it black
            if (i == selectedAction)
                actionTexts[i].color = highlightedColor;
            else
                actionTexts[i].color = Color.black;
        }
    }

    //Same thing but with highlighting moves
    public void UpdateMoveSelection(int selectedMove, Move move)
    {
        for (int i = 0; i < moveTexts.Count; ++i)
        {
            if (i == selectedMove)
                moveTexts[i].color = highlightedColor;
            else
                moveTexts[i].color = Color.black;
        }
        //Set the PP and move Type to the move
        ppText.text = $"PP {move.PP} / {move.Base.PP}";
        typeText.text = move.Base.Type.ToString();

        if (move.PP == 0)
            ppText.color = Color.red;
        else
            ppText.color = Color.black;
    }

    //Set the names of the moves to the moves the Pokemon knows
    public void SetMoveNames(List<Move> moves)
    {
        //Pokemon could know less than 4 moves
        for (int i = 0; i < moveTexts.Count; ++i)
        {
            //if i is less than amount of moves known, set that move text to the move [i] in list, otherwise set it to '-'
            if (i < moves.Count)
                moveTexts[i].text = moves[i].Base.Name;
            else
                moveTexts[i].text = "-";
        }
    }
}

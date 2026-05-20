using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.InputSystem;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] Text dialogText;
    [SerializeField] int lettersPerSecond;

    public event Action OnShowDialog;
    public event Action OnDialogFinished;

    public static DialogManager Instance { get; private set; }
    
    private void Awake()
    {
        Instance = this;
    }

    public bool  IsShowing { get; private set; }

    Coroutine typingCoroutine;


    public IEnumerator ShowDialogText(string text, bool waitForInput=true, bool autoClose=true)
    {

        OnShowDialog?.Invoke();
        IsShowing = true;
        dialogBox.SetActive(true);

        yield return TypeDialog(text);
        if (waitForInput)
        {
            yield return new WaitUntil(() => Keyboard.current.zKey.wasPressedThisFrame);
        }

        if (autoClose)
        {
            CloseDialog();
        }
        OnDialogFinished?.Invoke();
    }

    public void CloseDialog()
    {
        dialogBox.SetActive(false);
        IsShowing = false;
    }

    public IEnumerator ShowDialog(Dialog dialog)
    {
        yield return new WaitForEndOfFrame();  

        OnShowDialog?.Invoke();
        IsShowing = true;
        dialogBox.SetActive(true);

        foreach (var line in dialog.Lines)
        {
            yield return TypeDialog(line);
            yield return new WaitUntil(() => Keyboard.current.zKey.wasPressedThisFrame);
        }

        dialogBox.SetActive(false);
        IsShowing = false;
        OnDialogFinished?.Invoke();
    }

    public void HandleUpdate()
    {
        
    }

    // Begin typing dialog. If actively typing, cancel that dialog, start new one so no overlap
    public IEnumerator TypeDialog(string line)
    {
        dialogText.text = "";
        foreach (var letter in line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
    }
}

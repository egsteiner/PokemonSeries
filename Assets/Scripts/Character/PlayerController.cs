using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour, ISavable
{
    [SerializeField] string name;
    [SerializeField] Sprite sprite;

    

    private Vector2 inputMovement;

    private Character character;

    private PlayerMovement playerInput;

    //Remembers what axis was pressed most recently
    private Direction lastPressed = Direction.NONE;
    private bool hor = false;
    private bool ver = false;

    private void Awake()
    {
        playerInput = new PlayerMovement();
        character = GetComponent<Character>();

    }

    // Instead of normal update, have GameController decide which script gets to actively update, so battle and free roam controls don't happen simultaneously
    public void HandleUpdate()
    {
        //If we aren't moving...
        if (!character.IsMoving)
        {
            //Pull the value from the WASD input, normalized forces it to be length 1 (W is (0,1), A is (-1,0), etc)
            inputMovement = playerInput.Player.Move.ReadValue<Vector2>().normalized;

            //rounds the input number to 1 or -1 to keep consistent with 1 tile movements
            //IDK if this is necessary with the above line, but this ensures it
            if (inputMovement.x > 0) inputMovement.x = 1;
            if (inputMovement.x < 0) inputMovement.x = -1;
            if (inputMovement.y > 0) inputMovement.y = 1;
            if (inputMovement.y < 0) inputMovement.y = -1;

            //Removes diagonal moving

            //If moving in y-axis and horizontal movement is false...
            if (inputMovement.y != 0 && !hor)
            {
                //vertical movement is true, input of x-axis will be 0 as long as y-axis is moving
                ver = true;
                inputMovement.x = 0;
            }
            //Once not moving, vertical movement back to false
            else ver = false;

            //If already moving in x-axis and vertical movement is false...
            if (inputMovement.x != 0 && !ver)
            {
                //horizontal movement is true, inputs in y-axis will always be 0
                hor = true;
                inputMovement.y = 0;
            }
            //once stop moving, back to false
            else hor = false;

            //Set target position
            //If we are receiving input in some direction...
            if (inputMovement != Vector2.zero)
            {
                StartCoroutine(character.Move(inputMovement, OnMoveOver));
            }
        }

        character.HandleUpdate();

        if (Keyboard.current.zKey.wasPressedThisFrame)
            StartCoroutine(Interact());
    }

    IEnumerator Interact()
    {
        var facingDir = new Vector3(character.Animator.MoveX, character.Animator.MoveY);
        var interactPos = transform.position + facingDir;

        //Debug.DrawLine(transform.position, interactPos, Color.green, 0.5f);

        Physics2D.OverlapCircle(interactPos, 0.3f, GameLayers.i.InteractableLayer);

        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, GameLayers.i.InteractableLayer);
        if(collider != null)
        {
            yield return collider.GetComponent<Interactable>()?.Interact(transform);
        }
    }

    //When Player bcomes active
    private void OnEnable()
    {
        //Listen for Inputs
        playerInput.Enable();
    }

    //When Player is  deactivated
    private void OnDisable()
    {
        //Stop listening for Inputs
        playerInput.Disable();
    }

    IPlayerTriggerable currentlyInTrigger;

    private void OnMoveOver()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position - new Vector3(0, character.OffsetY), 0.2f, GameLayers.i.TriggerableLayers);

        IPlayerTriggerable triggerable = null; ;
        foreach (var  collider in colliders)
        {
            triggerable = collider.GetComponent<IPlayerTriggerable>();
            if (triggerable != null)
            {
                if (triggerable == currentlyInTrigger && !triggerable.TriggerRepeatedly)
                    break;
                
                triggerable.OnPlayerTriggered(this);
                currentlyInTrigger = triggerable;
                break;
            }
        }

        if (colliders.Count() == 0 || triggerable != currentlyInTrigger)
            currentlyInTrigger = null;
    }

    public object CaptureState()
    {
        var saveData = new PlayerSaveData()
        {
            position = new float[] { transform.position.x, transform.position.y },
            pokemons = GetComponent<PokemonParty>().Pokemons.Select(p => p.GetSaveData()).ToList()
        };

        return saveData;
    }

    public void RestoreState(object state)
    {
        var saveData = (PlayerSaveData)state;

        // Restore position
        var pos = saveData.position;
        //transform.position = new Vector3(pos[0], pos[1]);
        character.SetPositionAndSnapToTile(new Vector2(pos[0], pos[1]));

        //Restore party
        GetComponent<PokemonParty>().Pokemons = saveData.pokemons.Select(s => new Pokemon(s)).ToList();
        Physics2D.SyncTransforms();
    }


    //Constants of type Direction, in X and Y directions, or NONE
    private enum Direction
    {
        NONE,
        X,
        Y
    }

    public string Name
    {
        get => name;
    }

    public Sprite Sprite
    {
        get => sprite;
    }

    public Character Character => character;
}

[Serializable]
public class PlayerSaveData
{
    public float[] position;
    public List<PokemonSaveData> pokemons;
}



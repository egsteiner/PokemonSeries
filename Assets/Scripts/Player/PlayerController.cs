using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public LayerMask solidObjectsLayer;
    public LayerMask grassLayer;

    public event Action OnEncountered;

    private bool isMoving;
    private Vector2 inputMovement;

    private Animator animator;

    private PlayerMovement playerInput;

    //Remembers what axis was pressed most recently
    private Direction lastPressed = Direction.NONE;
    private bool hor = false;
    private bool ver = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerInput = new PlayerMovement();

    }

    // Instead of normal update, have GameController decide which script gets to actively update, so battle and free roam controls don't happen simultaneously
    public void HandleUpdate()
    {
        //If we aren't moving...
        if (!isMoving)
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
                //Set our animator bools to check which direction player moving, to switch to correct sprite
                animator.SetFloat("moveX", inputMovement.x);
                animator.SetFloat("moveY", inputMovement.y);

                //Set target position to current position
                var targetPos = transform.position;
                //Add the distance to the tile we want to move to (Ex: if moving to right, targetPos would have +1 to x-axis, same y-axis as transform.position
                targetPos.x += inputMovement.x;
                targetPos.y += inputMovement.y;

                //If this new tile is walkable, start coroutine of Move (Coroutine is function that runs over multiple frames instead of all at once)
                if (IsWalkable(targetPos)) StartCoroutine(Move(targetPos));
            }
        }

        animator.SetBool("isMoving", isMoving);
    }

    //Move player to target position
    //IEnumerator is something that can be ran step by step. Lets code pause and resume later, continuing where it left off. Used with coroutines
    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        
        //While the distance between our target tile and player  position is more than Mathf.Epsilon(extremely tiny number)
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            //Move from player position to targetPosition, (moveSpeed * Time.deltaTime) moves independently from frame-rate
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            //Prevents player from "teleporting" to a tile, instead  move tiny bit each frame
            yield return null;
        }
        transform.position = targetPos;

        isMoving = false;

        //Check for encounters after moving to each tile
        CheckForEncounters();
    }

    //Check if target position is walkable
    private bool IsWalkable(Vector3 targetPos)
    {
        //Make small circle  around player that checks if the tile we want to walk to has a solid object in it. If so, tile is not walkable.
        if (Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer) != null)
        {
            return false;
        }

        return true;

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

    //Check if we in grass tile, run encounter chance if so
    private void CheckForEncounters()
    {
        //Small circle radius to check if current tile is grass
        if (Physics2D.OverlapCircle(transform.position, 0.2f, grassLayer) != null)
        {
            //If we are in the  grass tile, 10% chance of encountering a Pokemon
            if (UnityEngine.Random.Range(1, 101) <= 10)
            {
                animator.SetBool("isMoving", false);
                OnEncountered();
            }
        }
    }

    //Constants of type Direction, in X and Y directions, or NONE
    private enum Direction
    {
        NONE,
        X,
        Y
    }
}

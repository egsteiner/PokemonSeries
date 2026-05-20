using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class Character : MonoBehaviour
{
    public float moveSpeed;

    public bool IsMoving { get; set; }

    public float OffsetY { get; private set; } = 0.3f;

    CharacterAnimator animator;

    private void Awake()
    {
        animator = GetComponent<CharacterAnimator>();
        SetPositionAndSnapToTile(transform.position);
    }

    public void SetPositionAndSnapToTile(Vector2 pos)
    {
        pos.x = Mathf.Floor(pos.x) + 0.5f;
        pos.y = Mathf.Floor(pos.y) + 0.5f + OffsetY;

        transform.position = pos;
    }

    //Move player to target position
    //IEnumerator is something that can be ran step by step. Lets code pause and resume later, continuing where it left off. Used with coroutines
    public IEnumerator Move(Vector2 moveVec, Action OnMoveOver=null)
    {

        //Set our animator bools to check which direction player moving, to switch to correct sprite
        animator.MoveX = Mathf.Clamp(moveVec.x, -1f, 1f);
        animator.MoveY = Mathf.Clamp(moveVec.y, -1f, 1f);

        //Set target position to current position
        var targetPos = transform.position;
        //Add the distance to the tile we want to move to (Ex: if moving to right, targetPos would have +1 to x-axis, same y-axis as transform.position
        targetPos.x += moveVec.x;
        targetPos.y += moveVec.y;

        if (!IsPathClear(targetPos))
        {
            yield break;
        }

        IsMoving = true;

        //While the distance between our target tile and player  position is more than Mathf.Epsilon(extremely tiny number)
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            //Move from player position to targetPosition, (moveSpeed * Time.deltaTime) moves independently from frame-rate
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            //Prevents player from "teleporting" to a tile, instead  move tiny bit each frame
            yield return null;
        }

        transform.position = targetPos;

        IsMoving = false;

        //Check for encounters after moving to each tile

        OnMoveOver?.Invoke();
    }

    public void HandleUpdate()
    {
        animator.IsMoving = IsMoving;
    }

    private bool IsPathClear(Vector3 targetPos)
    {
        var diff = targetPos - transform.position;
        var dir = diff.normalized;
        if (Physics2D.BoxCast(transform.position + dir, new Vector2(0.2f, 0.2f), 0f, dir, diff.magnitude - 1, GameLayers.i.SolidLayer | GameLayers.i.InteractableLayer | GameLayers.i.PlayerLayer) == true)
        {
            
            return false;
        }

        return true;
    }

    //Check if target position is walkable
    private bool IsWalkable(Vector3 targetPos)
    {
        //Make small circle  around player that checks if the tile we want to walk to has a solid object in it. If so, tile is not walkable.
        if (Physics2D.OverlapCircle(targetPos, 0.2f, GameLayers.i.SolidLayer | GameLayers.i.InteractableLayer) != null)
        {
            return false;
        }

        return true;

    }

    public void LookTowards(Vector3 targetPos)
    {
        var xdiff = Mathf.Floor(targetPos.x) - Mathf.Floor(transform.position.x);
        var ydiff = Mathf.Floor(targetPos.y) - Mathf.Floor(transform.position.y);

        if (xdiff == 0 || ydiff == 0)
        {
            animator.MoveX = Mathf.Clamp(xdiff, -1f, 1f);
            animator.MoveY = Mathf.Clamp(ydiff, -1f, 1f);
        }
        else
            Debug.LogError("Error in Look Towards: Can't ask character to look diagonally!!!");
    }

    public CharacterAnimator Animator
    {
        get => animator;
    }
}

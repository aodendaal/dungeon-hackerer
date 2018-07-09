using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private bool hasNewPosition = false;
    private bool canMove = true;

    private Vector3 newPosition;
    private float newDirection;

    private float moveSpeed = 0.3f;
    private float turnSpeed = 0.3f;    

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (canMove)
        {
            if (hasNewPosition)
            {
                transform.position = newPosition;
                DungeonData.PlayerPosition = transform.position;
                transform.rotation = Quaternion.Euler(0, newDirection, 0);

                hasNewPosition = false;
            }

            if (!DungeonData.inDebugMode)
            {
                if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
                {
                    MoveForward();
                }

                if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
                {
                    MoveBackward();
                }

                if (Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.RightArrow))
                {
                    TurnRight();
                }

                if (Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.LeftArrow))
                {
                    TurnLeft();
                }

                if (Input.GetKeyUp(KeyCode.A))
                {
                    MoveLeft();
                }

                if (Input.GetKeyUp(KeyCode.D))
                {
                    MoveRight();
                }
            } 
        }
    }

    public void SetNewPosition(Vector3 position, float direction)
    {
        hasNewPosition = true;
        newPosition = position;
        newDirection = direction;
    }

    private Direction GetFacing()
    {
        var angle = transform.rotation.eulerAngles.y;

        if (angle == 0.0f) return Direction.Up;
        if (angle == 90.0f) return Direction.Right;
        if (angle == -90.0f || angle == 270.0f) return Direction.Left;

        return Direction.Down;
    }

    private bool CanMoveToPosition(Vector3 newPosition)
    {
        var ray = new Ray(transform.position, newPosition - transform.position);

        if (Physics.Raycast(ray, 5, 1 << 8))
        {
            return false;
        }

        return true;
    }

    private void MoveForward()
    {
        var newPos = transform.position + transform.forward * 4.0f;
        if (!CanMoveToPosition(newPos))
        {
            return;
        }

        canMove = false;
        DungeonData.PlayerPosition = newPos;

        LeanTween.moveLocal(gameObject, newPos, moveSpeed)
                    .setOnComplete(() => canMove = true);
    }

    private void MoveBackward()
    {
        var newPos = transform.position - transform.forward * 4.0f;
        if (!CanMoveToPosition(newPos))
        {
            return;
        }

        canMove = false;
        DungeonData.PlayerPosition = newPos;

        LeanTween.moveLocal(gameObject, newPos, moveSpeed)
                    .setOnComplete(() => canMove = true);
    }

    private void MoveLeft()
    {
        var newPos = transform.position - transform.right * 4.0f;
        if (!CanMoveToPosition(newPos))
        {
            return;
        }

        canMove = false;
        DungeonData.PlayerPosition = newPos;

        LeanTween.moveLocal(gameObject, newPos, moveSpeed)
                    .setOnComplete(() => canMove = true);
    }

    private void MoveRight()
    {
        var newPos = transform.position + transform.right * 4.0f;
        if (!CanMoveToPosition(newPos))
        {
            return;
        }

        canMove = false;
        DungeonData.PlayerPosition = newPos;

        LeanTween.moveLocal(gameObject, newPos, moveSpeed)
                    .setOnComplete(() => canMove = true);
    }

    private void TurnRight()
    {
        canMove = false;

        LeanTween.rotateY(gameObject, transform.rotation.eulerAngles.y + 90.0f, turnSpeed)
                    .setOnComplete(() => canMove = true);
    }

    private void TurnLeft()
    {
        canMove = false;

        LeanTween.rotateY(gameObject, transform.rotation.eulerAngles.y - 90.0f, turnSpeed)
                    .setOnComplete(() => canMove = true);
    }
}

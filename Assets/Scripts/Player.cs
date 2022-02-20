using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class Player : MonoBehaviour

{
    public static Player Instance;

    private Vector2 mousePressStart;
    private Vector3 currentMovementDirection = Vector3.zero;
    private Vector3 paintingVector = new Vector3(0, -1, 0);

    [SerializeField] private Transform playerSpawn;

    [SerializeField] private float rayMaxDistance;
    [SerializeField] private float speed;
    [SerializeField] private float swipeSensitivity;
    [SerializeField] private float swipeDirectionSensitivity;

    RaycastHit hit;

    Rigidbody rb;

    Color32 playerColor;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ChangePlayerColor();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePressStart = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 dragVectorDirection = ((Vector2)Input.mousePosition - mousePressStart).normalized;
            SetMovement(GetDragDirection(dragVectorDirection));
        }

        if(currentMovementDirection != Vector3.zero && Physics.Raycast(transform.position, currentMovementDirection, rayMaxDistance))
        {
            SetMovement(MovementDirection.Stop);
        }

        if (currentMovementDirection != Vector3.zero)
        {
            if (Physics.Raycast(transform.position, paintingVector, out hit))
            {
                Renderer renderer = hit.transform.GetComponent<Renderer>();
                if (renderer.material.color != playerColor)
                {
                    renderer.material.color = playerColor;
                    GameManager.Instance.PaintedGround(renderer);
                }
            }
        }
    }
    private enum MovementDirection
    {
        Up,
        Down,
        Right,
        Left,
        Stop
    }
    private MovementDirection GetDragDirection(Vector3 dragVector)
    {
        float positiveX = Mathf.Abs(dragVector.x);
        float positiveY = Mathf.Abs(dragVector.y);
        MovementDirection draggedDir;
        if (positiveX > positiveY)
        {
            draggedDir = (dragVector.x > 0) ? MovementDirection.Right : MovementDirection.Left;
        }
        else
        {
            draggedDir = (dragVector.y > 0) ? MovementDirection.Up : MovementDirection.Down;
        }
        return draggedDir;
    }

    private void SetMovement(MovementDirection direction)
    {
        if (direction != MovementDirection.Stop && currentMovementDirection == Vector3.zero)
        {
            Vector3 vector;
            
            
            if (direction == MovementDirection.Up)
            {
                vector = new Vector3(0, 0, speed);
                if (!Physics.Raycast(transform.position, vector, rayMaxDistance))
                {
                    rb.velocity = vector;
                    currentMovementDirection = vector.normalized;
                }
            }
            else if (direction == MovementDirection.Down)
            {
                vector = new Vector3(0, 0, -speed);
                if (!Physics.Raycast(transform.position, vector, rayMaxDistance))
                {
                    rb.velocity = vector;
                    currentMovementDirection = vector.normalized;
                }
            }
            else if (direction == MovementDirection.Left)
            {
                vector = new Vector3(-speed, 0, 0);
                if (!Physics.Raycast(transform.position, vector, rayMaxDistance))
                {
                    rb.velocity = vector;
                    currentMovementDirection = vector.normalized;
                }
            }
            else if (direction == MovementDirection.Right)
            {
                vector = new Vector3(speed, 0, 0);
                if (!Physics.Raycast(transform.position, vector, rayMaxDistance))
                {
                    rb.velocity = vector;
                    currentMovementDirection = vector.normalized;
                }
            }
            
        }

        else if (direction == MovementDirection.Stop)
        {
            rb.velocity = Vector3.zero;
            currentMovementDirection = Vector3.zero;
        }
    }
    
    public void PlayerReset()
    {
        ChangePlayerColor();
        SetMovement(MovementDirection.Stop);
        transform.position = playerSpawn.position;
    }
    public void ChangePlayerColor()
    {
        playerColor = new Color32((byte)Random.Range(0, 256), (byte)Random.Range(0, 256), (byte)Random.Range(0, 256), 255);
        transform.GetComponent<Renderer>().material.color = playerColor;
    }
}

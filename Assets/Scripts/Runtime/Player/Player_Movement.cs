using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Character Controller vars")]
    [SerializeField] private Player player;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private float gravity = -30f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;
    private bool isGrounded;
    private Vector3 moveDirection;

    [Header("Rotation vars")]
    [SerializeField] private float _rotSpeed = 20;
    [SerializeField] private float _verticalSpeed = 10f;
    [SerializeField] private float _verticalSpeedNeg = 5f;
    float rotX;

    [HideInInspector] public bool dead;
    private float lastMovement;
    public float movement;
    public bool isMoving;
    public bool isRunning;
    public bool isRotating;
    public bool canMove = true;
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Input.GetAxis("Vertical"));
        //Debug.Log("Raw :" + Input.GetAxisRaw("Vertical"));

        //if (!GameHandler.instance.IsPaused() && !dead)
        //{
        //    isRunning = false;
        //    isRotating = false;
        //    if (!player.stopControlls && canMove)
        //    {
        //        RotatePlayerWithMouse();
        //        Movement();
        //        movement = Input.GetAxis("Vertical");
        //        if (movement == 0) isMoving = false; else isMoving = true;
        //        if (Input.GetAxis("Mouse X") != 0) { isRotating = true; }
        //    }
        //}
    }

    private void FixedUpdate()
    {
        if (!GameHandler.instance.IsPaused() && !dead)
        {
            isRunning = false;
            isRotating = false;
            if (!player.stopControlls && canMove)
            {
                RotatePlayerWithMouse();
                Movement();
                lastMovement = movement;
                movement = Input.GetAxis("Vertical");
                if (movement == 0 && lastMovement == 0) isMoving = false; else isMoving = true;
                if (Input.GetAxis("Mouse X") != 0) { isRotating = true; }
            }
        }
    }

    void Movement()
    {
        float verticalMove = 0;
        moveDirection.x = 0;
        moveDirection.z = 0;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && moveDirection.y < 0) { moveDirection.y = -2f; }
        if (Input.GetAxis("Vertical") > 0.1)
        {
            verticalMove = Input.GetAxis("Vertical") * Time.deltaTime * _verticalSpeed;
            if (Input.GetKey(KeyCode.LeftShift)) { isRunning = true; verticalMove *= 2;  }
            moveDirection = transform.TransformDirection(Vector3.forward) * verticalMove;
        }
        else if (Input.GetAxis("Vertical") < -0.1)
        {
            verticalMove = Input.GetAxis("Vertical") * Time.deltaTime * _verticalSpeedNeg;
            moveDirection = transform.TransformDirection(Vector3.forward) * verticalMove;
        }
        moveDirection.y += gravity * Time.deltaTime;
        _characterController.Move(moveDirection);
    }

    void RotatePlayerWithMouse()
    {
        rotX = Input.GetAxis("Mouse X") * _rotSpeed * Mathf.Deg2Rad;
        transform.Rotate(Vector3.up, rotX);
    }
}

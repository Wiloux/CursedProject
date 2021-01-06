using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    // Start is called before the first frame update

    float rotX;
    public CharacterController _characterController;
    public float _rotSpeed = 20;
    public float _verticalSpeed = 10f;
    public float _verticalSpeedNeg = 5f;

    public bool stopControlls;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!stopControlls)
        {
            RotatePlayerWithMouse();
            Movement();

            if (Input.GetKeyDown(KeyCode.E))
            {
                RaycastHit hit;
                Physics.Raycast(transform.position, transform.forward, out hit, 5f);
                if (hit.transform != null)
                {
                    if(hit.transform.GetComponent<DoorScript>() != null) hit.transform.GetComponent<DoorScript>().UseDoor(transform);
                    else if (hit.transform.CompareTag("SavePoint"))
                    {
                        GameHandler.instance.TogglePause();
                        GameHandler.instance.ToggleSaveMenu();
                    }
                }
            }
        }
    }

        void Movement()
        {
            float verticalMove = 0;
            Vector3 moveDirection;
            if (Input.GetAxis("Vertical") > 0.1)
            {
                verticalMove = Input.GetAxis("Vertical") * Time.deltaTime * _verticalSpeed;
                moveDirection = transform.TransformDirection(Vector3.forward) * verticalMove;
                _characterController.Move(moveDirection);

            }

            if (Input.GetAxis("Vertical") < -0.1)
            {
                verticalMove = Input.GetAxis("Vertical") * Time.deltaTime * _verticalSpeedNeg;
                moveDirection = transform.TransformDirection(Vector3.forward) * verticalMove;
                _characterController.Move(moveDirection);
            }
        }

        void RotatePlayerWithMouse()
        {
            rotX = Input.GetAxis("Mouse X") * _rotSpeed * Mathf.Deg2Rad;
            transform.Rotate(Vector3.up, rotX);
        }
    }

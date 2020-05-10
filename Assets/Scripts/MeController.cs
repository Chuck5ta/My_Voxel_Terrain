using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeController : MonoBehaviour
{
    Rigidbody rigidBody;
    public float moveSpeed = 1;
    public float sensitivity = 1;
    Vector3 euler;

    // Start is called before the first frame update
    void Start()
    {
        // get the RigidBody of the game object this script is attached to
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    // https://answers.unity.com/questions/666713/rotating-object-with-mouse-axis-is-very-choppy.html
    void FixedUpdate()
    {
        // add check for right mouse button down?
        // rotations - character looking around
        euler.x += Input.GetAxis("Mouse Y") * sensitivity;
        euler.y -= Input.GetAxis("Mouse X") * sensitivity;
        rigidBody.transform.rotation = Quaternion.Euler(euler);
        rigidBody.velocity = transform.forward * Input.GetAxis("Vertical") * moveSpeed;
    }

    private void ProcessInput()
    {
        // W forward
        // D backward
        // Right Mouse button held down - left/right/up/down movement
        if (Input.GetKey(KeyCode.W))
        {
            // move in the direction the game object is facing (AddRelativeForce)
            rigidBody.AddRelativeForce(Vector3.forward);
            print("Forward movement");
        }
        else if (Input.GetKey(KeyCode.S))
        {
            // move in the reverse direction the game object is facing (AddRelativeForce)
            rigidBody.AddRelativeForce(Vector3.back);
            print("Backward movement");
        }
        // MOVE LEFT (strafe)
        else if (Input.GetKey(KeyCode.A))
        {
            // move in the reverse direction the game object is facing (AddRelativeForce)
            rigidBody.AddRelativeForce(Vector3.left);
            print("Backward movement");
        }
        // MOVE RIGHT (strafe)
        else if (Input.GetKey(KeyCode.D))
        {
            // move in the reverse direction the game object is facing (AddRelativeForce)
            rigidBody.AddRelativeForce(Vector3.right);
            print("Backward movement");
        }
        else
        {
            // else STOP movement
            rigidBody.velocity = Vector3.zero;
        }

        // rotate
        if (Input.GetKey(KeyCode.Mouse1))
        {
            print("Right mouse button");

            // ROTATE LEFT
            if (Input.GetAxis("Mouse X") < 0) // mouse moving left
            {
                print("Mouse moving left");
                transform.Rotate(Vector3.down); // rotate on the Y axis (LEFT/RIGHT - Yaw)
                                                //    transform.Rotate(Vector3.left); // rotate on the X axis (UP/DOWN - Pitch)
                                                //    transform.Rotate(Vector3.forward); // rotate on the Z axis (Roll)
            }
            // ROTATE RIGHT
            else if (Input.GetAxis("Mouse X") > 0) // mouse moving left
            {
                print("Mouse moving right");
                transform.Rotate(Vector3.up); // rotate on the Y axis (LEFT/RIGHT - Yaw)
                                              //    transform.Rotate(Vector3.left); // rotate on the X axis (UP/DOWN - Pitch)
                                              //    transform.Rotate(Vector3.forward); // rotate on the Z axis (Roll)
            }

        }

    }
}

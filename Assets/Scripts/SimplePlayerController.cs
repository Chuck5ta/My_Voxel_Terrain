using UnityEngine;

public class SimplePlayerController : MonoBehaviour
{
    Rigidbody rigidBody;
    public float moveSpeed = 5;
    public float sensitivity = 1;
    Vector3 euler; // (oiler) used in the calculation for rotating the player character

    // Start is called before the first frame update
    void Start()
    {
        // get the RigidBody of the game object this script is attached to
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeDirection();
        MovePlayer();
    }

    /*
     * This rotates the player and camera to look around and change movement direction
     * TODO: Not working with movement (forward, backward, side, etc.)
     */
    private void ChangeDirection()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            // rotations - character looking around
            euler.y += Input.GetAxis("Mouse X") * sensitivity;
            euler.x -= Input.GetAxis("Mouse Y") * sensitivity;
            rigidBody.transform.rotation = Quaternion.Euler(euler);
            rigidBody.velocity = transform.forward * Input.GetAxis("Vertical") * moveSpeed;
        }
       // MovePlayer();
    }

    // https://docs.unity3d.com/ScriptReference/Transform-right.html
    private void MovePlayer()
    {
        // MOVE FORWARDS
        if (Input.GetKey(KeyCode.W))
        {
            rigidBody.velocity = transform.forward * moveSpeed;
        }
        // MOVE BACKWARDS
        else if (Input.GetKey(KeyCode.S))
        {
            rigidBody.velocity = -transform.forward * moveSpeed;
        }
        // MOVE LEFT (strafe)
        else if (Input.GetKey(KeyCode.A))
        {
            rigidBody.velocity = -transform.right * moveSpeed;
        }
        // MOVE RIGHT (strafe)
        else if (Input.GetKey(KeyCode.D))
        {
            rigidBody.velocity = transform.right * moveSpeed;
        }
        // MOVE UP
        else if (Input.GetKey(KeyCode.R))
        {
            rigidBody.velocity = transform.up * moveSpeed;
        }
        // MOVE DOWN
        else if (Input.GetKey(KeyCode.F))
        {
            rigidBody.velocity = -transform.up * moveSpeed;
        }
        else
        {
            // else STOP movement
            rigidBody.velocity = Vector3.zero;
        }
    }
}

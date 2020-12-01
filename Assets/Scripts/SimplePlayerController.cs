using UnityEngine;

public class SimplePlayerController : MonoBehaviour
{
    public GameObject player;
    Rigidbody rigidBody;
    public float moveSpeed = 5;
    public float turningSpeed = 1;
 //  Vector3 euler; // (oiler) used in the calculation for rotating the player character // TODO - Not used at the moment

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
            float h = 2.0f * Input.GetAxis("Mouse X");
            float v = 2.0f * Input.GetAxis("Mouse Y");
            transform.Rotate(v, -h, 0);
        }
    }


    // https://docs.unity3d.com/ScriptReference/Transform-right.html
    private void MovePlayer()
    {
        // MOVE FORWARDS
        if (Input.GetKey(KeyCode.W))
        {
        //    transform.Translate(new Vector3(0, 0, moveSpeed * Time.deltaTime));
            rigidBody.velocity = transform.forward * moveSpeed;
        }
        // MOVE BACKWARDS
        else if (Input.GetKey(KeyCode.S))
        {
            //transform.Translate(new Vector3(0, 0, -moveSpeed * Time.deltaTime));
            rigidBody.velocity = -transform.forward * moveSpeed;
        }
        // MOVE LEFT (strafe)
        else if (Input.GetKey(KeyCode.A))
        {
    //        transform.Translate(new Vector3(-moveSpeed * Time.deltaTime, 0, 0));
            rigidBody.velocity = -transform.right * moveSpeed;
        }
        // MOVE RIGHT (strafe)
        else if (Input.GetKey(KeyCode.D))
        {
      //      transform.Translate(new Vector3(moveSpeed * Time.deltaTime, 0, 0));
            rigidBody.velocity = transform.right * moveSpeed;
        }
        // MOVE UP
        else if (Input.GetKey(KeyCode.R))
        {
      //      transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
            rigidBody.velocity = transform.up * moveSpeed;
        }
        // MOVE DOWN
        else if (Input.GetKey(KeyCode.F))
        {
      //      transform.Translate(new Vector3(0, -moveSpeed * Time.deltaTime, 0));
            rigidBody.velocity = -transform.up * moveSpeed;
        }
        // Turn anticlockwise
        else if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(new Vector3(0, 0, 1), Space.Self);
        }
        // Turn clockwise
        else if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(new Vector3(0, 0, -1), Space.Self);
        }
        else
        {
            // else STOP movement
            rigidBody.velocity = Vector3.zero;
        }
    }
}

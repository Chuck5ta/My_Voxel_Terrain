using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float inputDelay = 0.1f; // a delay before the action (key press) is acted on
    public float forwardVelocity = 12;
    public float rotateVelocity = 100;

    Quaternion targetRotation;
    Rigidbody rBody;
    float forwardInput, turnInput; // for forwardVelocity and rotateVelocity respectively

    /*
     * This is access by the CameraController
     */
    public Quaternion TargetRotation
    {
        get { return targetRotation;  }
    }

    private void Start()
    {
        targetRotation = transform.rotation;
        if (GetComponent<Rigidbody>())
            rBody = GetComponent<Rigidbody>();
        else
            Debug.LogError("Rigidbody is missing from the character!");

        forwardInput = turnInput = 0;        
    }

    void GetInput()
    {
        // see @ 6 mins 40 on the vid
        forwardInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");

    }

    private void Update()
    {
        GetInput();
        Turn();
    }

    private void FixedUpdate()
    {
        Run();
    }

    void Run()
    {
        if (Mathf.Abs(forwardInput) > inputDelay)
        {
            // Move
            rBody.velocity = transform.forward * forwardInput * forwardVelocity;
        }
        else
            // Stop/slow
            rBody.velocity = Vector3.zero;

    }

    void Turn()
    {
        if (Mathf.Abs(turnInput) > inputDelay)
        {
            targetRotation *= Quaternion.AngleAxis(rotateVelocity + turnInput * Time.deltaTime, Vector3.up);
        }
        transform.rotation = targetRotation;
    }
}

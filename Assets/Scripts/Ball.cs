using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
    private Rigidbody myBody;
    private InputAction bowlButton;
    
    private bool holdingButton; // making a state machine, in games you want to have discrete states.
                              // in this game, we have three. at rest, powering up, rolling. making two of them for now
    private bool roll;

    public float rollPower;
    public LayerMask personLayer;

    private List<Vector3> pinPositions; // will make it possible to reset
    private List<Quaternion> pinRotations; // will make it possible to reset
    private Vector3 ballPosition;

    public GameObject ball;

    void Start()
    {
        myBody = GetComponent<Rigidbody>();
        bowlButton = InputSystem.actions.FindAction("Bowl");
        
        var pins = GameObject.FindGameObjectsWithTag("Pin");
        pinPositions = new List<Vector3>();
        pinRotations = new List<Quaternion>();
        foreach (var pin in pins) // recording each pin's position & rotation
        {
            pinPositions.Add(pin.transform.position);
            pinRotations.Add(pin.transform.rotation);
        }

        ballPosition = GameObject.FindGameObjectWithTag("Ball").transform.position;

        //myBody.AddForce(transform.forward * 1000f); // the thing we are doing to it
    }

    void Update() // we do not want physics stuff in here !! bad !! wrong !!
    {
        if (bowlButton.WasPressedThisFrame())
        {
            holdingButton = true;
        }
        else if (bowlButton.WasReleasedThisFrame())
        {
            holdingButton = false;
            roll = true;
        }

        if (UnityEngine.InputSystem.Keyboard.current.rKey.wasPressedThisFrame || ball.transform.position.y < -10)
        {
            ResetPins();
        }

    }

    void FixedUpdate() // this is where the physics are !!! holy shit....... 
    {
        if (holdingButton && myBody.linearVelocity == Vector3.zero) // checking to see that the ball isn't already in motion.
        {
         // find the direction the mouse is pointing towards
         
         Debug.DrawRay(transform.position, FindDirection() * 1000f, Color.chartreuse);
        }

        if (roll)
        {
            StartRoll(); // roll the ball based on the mouse's direction
        }
    }

    void StartRoll()
    {
        if (Physics.Raycast(transform.position, FindDirection(), 1000f, personLayer) == false) 
            // makes it so you cannot throw the ball at a person.
        {
            myBody.isKinematic = false;
            myBody.AddForce(rollPower * transform.TransformDirection(FindDirection()), ForceMode.Impulse);
            roll = false;
        }
    }

    Vector3 FindDirection() // no void, we want to return a vector 3
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        mousePosition.z = 10f; // cannot be 0 unless we want everything to get messed up for some reason
        Vector3 direction = Camera.main.ScreenToWorldPoint(mousePosition);
        direction.z = 10f;
        return direction.normalized;
    }
    
    public void ResetPins()
    {
        StartCoroutine(ResetStage());
    }

    IEnumerator ResetStage()
    {var pins = GameObject.FindGameObjectsWithTag("Pin"); // find all pins & apply these settings to them
        for (int i = 0; i < pins.Length; i++)
        {
            yield return new WaitForSeconds(2);
            var pinPhysics = pins[i].GetComponent<Rigidbody>();
            pinPhysics.linearVelocity = Vector3.zero;
            pinPhysics.position = pinPositions[i];
            pinPhysics.rotation = pinRotations[i];
            pinPhysics.linearVelocity = Vector3.zero; // keeps pins from spinning into the void
            pinPhysics.angularVelocity = Vector3.zero; // ^
                    
            var ball = GameObject.FindGameObjectWithTag("Ball");
            ball.transform.position = ballPosition;
            ball.GetComponent<Rigidbody>().linearVelocity = Vector3.zero; // same thing but for the ball
            ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero; // ^
        }
    }
}
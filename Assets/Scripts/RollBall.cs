using UnityEngine;
using UnityEngine.InputSystem;

public class RollBall : MonoBehaviour
{
    
    public Transform  startingPosition;
    
    public Rigidbody rigidBody;

    private InputAction bowlButt;

    private bool holdingButt;
    private bool roll;

    public float rollPower;

    public LayerMask personLayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        bowlButt = InputSystem.actions.FindAction("Bowl");
        //rigidBody.AddForce(transform.forward * 1000); //GIVE HER FORCE LET HER MOVE! 
        //how does it know forward? Everything in Unity has a forward direction. No matter the shape. 
    }

    // Update is called once per frame
    //void means empty. By calling void, we are saying "return nothing."
    void Update()
    {
        if (bowlButt.WasPressedThisFrame())
        {
            holdingButt = true;
        



            //StartRoll();
        } else if (bowlButt.WasReleasedThisFrame())
        {
            holdingButt = false;
            roll = true;
        }
    }

    void FixedUpdate()
    {
        if (holdingButt && rigidBody.linearVelocity == Vector3.zero)
        {
            Debug.DrawRay(transform.position, FindDirection() * 1000f, Color.red); //visualize ray
            //find the direction the mouse is pointing towards
        }

        if (roll)
        {
            StartRoll();
            //roll the ball based on the mouse's direction
        }
    }

    void StartRoll()
    {
        if (Physics.Raycast(transform.position, FindDirection(), 1000f, personLayer) == false)
        {
            rigidBody.isKinematic = false;
            Vector3 pointTo = FindDirection();
            rigidBody.AddForce(rollPower * transform.TransformDirection(pointTo), ForceMode.Impulse);
            roll = false;
            
        }
        
    }

    Vector3 FindDirection()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        mousePosition.z = 10f;
        Vector3 direction = Camera.main.ScreenToWorldPoint(mousePosition);
        direction.z = 10f; 
        return direction;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BOUNDS"))
        {
            gameObject.transform.position = startingPosition.transform.position;
        }
    }

}

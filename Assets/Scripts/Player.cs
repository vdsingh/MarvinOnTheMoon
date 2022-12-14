using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    // Components
    private CharacterController characterController;

    // Player State (Variable)
    private bool playerIsGrounded;
    private Vector3 playerVelocity;
    private Vector3 movementDirection;

    // Player Constants
    private float walkingVelocity = 15.0f;
    private float jumpHeight = 5.0f;
    private float gravityValue = -5f; // This is the moon's gravity

    // The force that the user has when pushing things like blocks
    private float pushForce = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        characterController = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    // This function handles all movement associated with the player.
    void HandleMovement() {
        playerIsGrounded = characterController.isGrounded;
        if (playerIsGrounded && playerVelocity.y < 0) {
            playerVelocity.y = 0f;
        }

        if(Input.GetKey("w")) {
            float xdirection = Mathf.Sin(Mathf.Deg2Rad * transform.rotation.eulerAngles.y);
            float zdirection = Mathf.Cos(Mathf.Deg2Rad * transform.rotation.eulerAngles.y);
            movementDirection = new Vector3(xdirection, 0.0f, zdirection);

            characterController.Move(movementDirection * walkingVelocity * Time.deltaTime);
        }

        if(Input.GetKey("s")) {
            float xdirection = Mathf.Sin(Mathf.Deg2Rad * transform.rotation.eulerAngles.y);
            float zdirection = Mathf.Cos(Mathf.Deg2Rad * transform.rotation.eulerAngles.y);
            movementDirection = new Vector3(xdirection, 0.0f, zdirection);

            characterController.Move(-1 * movementDirection * walkingVelocity * Time.deltaTime);
        }


        // Changes the height position of the player.
        if(Input.GetKey(KeyCode.Space) && playerIsGrounded) {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        if(Input.GetKey("a")) {
            transform.Rotate(new Vector3(0, -90, 0) * Time.deltaTime);
        }

        if(Input.GetKey("d")) {
            transform.Rotate(new Vector3(0, 90, 0) * Time.deltaTime);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Block") {
            Debug.Log("Collision with Block");
            float pushStrength = pushForce;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(movementDirection * pushStrength, ForceMode.Impulse);
        }
    }
}

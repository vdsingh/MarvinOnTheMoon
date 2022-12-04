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
    private Vector3 movement_direction;

    // Player Constants
    private float walkingVelocity = 15.0f;
    private float jumpHeight = 10.0f;
    private float gravityValue = -1.62f; // This is the moon's gravity

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


        // Changes the height position of the player.
        if(Input.GetKey(KeyCode.Space) && playerIsGrounded) {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        if(Input.GetKey("a")) {
            Debug.Log("A down");
            transform.Rotate(new Vector3(0, -90, 0) * Time.deltaTime);
        }

        if(Input.GetKey("d")) {
            transform.Rotate(new Vector3(0, 90, 0) * Time.deltaTime);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }
}

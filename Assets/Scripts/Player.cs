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

    // Player Constants
    private float playerSpeed = 8.0f;
    private float jumpHeight = 1.0f;
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

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        characterController.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero) {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player.
        if (Input.GetKey(KeyCode.Space) && playerIsGrounded) {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        if(Input.GetKeyDown("a")) {
            transform.Rotate(new Vector3(0, -90, 0) * Time.deltaTime);
        }

        if(Input.GetKeyDown("d")) {
            transform.Rotate(new Vector3(0, 90, 0) * Time.deltaTime);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }
}

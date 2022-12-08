using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_Player : MonoBehaviour
{
    // Camera Variables
    public float horizontalSens = 1.0f;
    public float verticalSens = 1.0f;
    private float xRotation = 0.0f;
    private float yRotation = 0.0f;

    // Components
    private CharacterController characterController;
    public Camera camera;

    // Player State (Variable)
    private bool playerIsGrounded;
    private Vector3 playerVelocity;
    private Vector3 movementDirection;

    // Player Constants
    private float walkingVelocity = 15.0f;
    private float jumpHeight = 10.0f;
    private float gravityValue = -1.62f; // This is the moon's gravity


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        characterController = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Update_Camera();
        HandleMovement();
    }

    void Update_Camera()
    {
        float mouseX = Input.GetAxis("Mouse X") * horizontalSens;
        float mouseY = Input.GetAxis("Mouse Y") * verticalSens;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);
        yRotation += mouseX;

        camera.transform.eulerAngles = new Vector3(xRotation, yRotation, 0.0f);
    }

    void HandleMovement() {
        playerIsGrounded = characterController.isGrounded;
        if (playerIsGrounded && playerVelocity.y < 0) {
            playerVelocity.y = 0f;
        }

        if(Input.GetKey("w")) {
            float xdirection = Mathf.Sin(Mathf.Deg2Rad * camera.transform.rotation.eulerAngles.y);
            float zdirection = Mathf.Cos(Mathf.Deg2Rad * camera.transform.rotation.eulerAngles.y);
            movementDirection = new Vector3(xdirection, 0.0f, zdirection);

            characterController.Move(movementDirection * walkingVelocity * Time.deltaTime);
        }
        if (Input.GetKey("s")) {
            float xdirection = Mathf.Sin(Mathf.Deg2Rad * camera.transform.rotation.eulerAngles.y);
            float zdirection = Mathf.Cos(Mathf.Deg2Rad * camera.transform.rotation.eulerAngles.y);
            movementDirection = new Vector3(xdirection, 0.0f, zdirection);

            characterController.Move(-movementDirection * walkingVelocity * Time.deltaTime);
        }
        if (Input.GetKey("a")) {
            float xdirection = Mathf.Sin(Mathf.Deg2Rad * camera.transform.rotation.eulerAngles.y);
            float zdirection = Mathf.Cos(Mathf.Deg2Rad * camera.transform.rotation.eulerAngles.y);
            movementDirection = new Vector3(zdirection, 0.0f, xdirection);

            characterController.Move(-movementDirection * walkingVelocity * Time.deltaTime);
        }
        if (Input.GetKey("d")) {
            float xdirection = Mathf.Sin(Mathf.Deg2Rad * camera.transform.rotation.eulerAngles.y);
            float zdirection = Mathf.Cos(Mathf.Deg2Rad * camera.transform.rotation.eulerAngles.y);
            movementDirection = new Vector3(zdirection, 0.0f, xdirection);

            characterController.Move(movementDirection * walkingVelocity * Time.deltaTime);
        }


        // Changes the height position of the player.
        if(Input.GetKey(KeyCode.Space) && playerIsGrounded) {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }

}

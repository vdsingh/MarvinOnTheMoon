// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using UnityEditor;
// using TMPro;

// public class FPS_Player2 : MonoBehaviour
// {
//     // Camera Variables
//     public float horizontalSens = 1.0f;
//     public float verticalSens = 1.0f;
//     private float xRotation = 0.0f;
//     private float yRotation = 0.0f;

//     // Components
//     private CharacterController characterController;
//     public Camera fpsCamera;
//     public Camera thirdPersonCamera;
//     public RawImage object_icon;
//     public TMP_Text object_name;
//     public TMP_Text object_gravity;
//     public TMP_Text gravity_text;
//     public GameObject panel;

//     // Player State (Variable)
//     private bool playerIsGrounded;
//     private Vector3 playerVelocity;
//     private Vector3 movementDirection;

//     // Player Constants
//     private float range = 100.0f;
//     private float walkingVelocity = 15.0f;
//     private float runningVelocity = 30.0f;
//     private float jumpHeight = 10.0f;
//     private float gravityValue = -1.62f; // This is the moon's gravity


//     // Start is called before the first frame update
//     void Start()
//     {
//         Cursor.lockState = CursorLockMode.Locked;
//         Cursor.visible = false;
//         characterController = gameObject.GetComponent<CharacterController>();
//         Debug.Log(object_icon);
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         Update_Camera();
//         HandleMovement();
//         Looking_At_Object();
//         Scroll_Wheel_Gravity();
//     }

//     GameObject Find_Prefab_Object(GameObject obj)
//     {
//         while (obj.transform.parent != null)
//         {
//             obj = obj.transform.parent.gameObject;
//             if (obj.tag == "Prefab")
//             {
//                 return obj;
//             }
//         }
//         return obj;
//     }

//     void Scroll_Wheel_Gravity()
//     {
//         float amount = 0.1f;
//         if (Input.GetKey(KeyCode.LeftShift))
//         {
//             amount = 1.0f;
//         }
//         if (Input.GetAxis("Mouse ScrollWheel") > 0f)
//         {
//             gravityValue += amount;
//         }
//         else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
//         {
//             gravityValue -= amount;
//         }
//         gravity_text.text = "Gravity: " + -1 * Mathf.Round(gravityValue * 100) / 100;
//     }

//     void Looking_At_Object()
//     {
//         RaycastHit hit;
//         if (Physics.Raycast(GetComponent<Camera>().transform.position, GetComponent<Camera>().transform.forward, out hit, range))
//         {
//             panel.active = true;
//             GameObject obj = Find_Prefab_Object(hit.collider.gameObject);
//             Texture2D icon = (Texture2D)AssetPreview.GetAssetPreview(obj);
//             object_icon.texture = icon;
//             object_name.text = obj.name;
//             if (obj.GetComponent<Gravity>() != null)
//             {
//                 object_gravity.text = "Gravity: " + obj.GetComponent<Gravity>().gravity;
//             }
//             else
//             {
//                 object_gravity.text = "Gravity: N/A";
//             }
//         }
//         else
//         {
//             panel.active = false;
//         }
//     }

//     void Update_Camera()
//     {



//         //If the user clicks the ` key, switch the camera to a third person view: (Implemented by Vik)
//         if(Input.GetKeyDown("`")) {
//             if(GetComponent<Camera>().GetComponent<Camera>().enabled == true) {
//                 GetComponent<Camera>().GetComponent<Camera>().enabled = false;
//             } else {
//                 GetComponent<Camera>().GetComponent<Camera>().enabled = true;
//             }

//         }


//         float mouseX = Input.GetAxis("Mouse X") * horizontalSens;
//         float mouseY = Input.GetAxis("Mouse Y") * verticalSens;

//         xRotation -= mouseY;
//         xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);
//         yRotation += mouseX;

//         GetComponent<Camera>().transform.eulerAngles = new Vector3(xRotation, yRotation, 0.0f);
//         transform.eulerAngles = new Vector3(0.0f, yRotation, 0.0f);
//     }

//     void HandleMovement()
//     {
//         playerIsGrounded = characterController.isGrounded;
//         float moveSpeed;

//         if (playerIsGrounded && playerVelocity.y < 0)
//         {
//             playerVelocity.y = 0f;
//         }
//         float xdirection = Mathf.Sin(Mathf.Deg2Rad * transform.rotation.eulerAngles.y);
//         float zdirection = Mathf.Cos(Mathf.Deg2Rad * transform.rotation.eulerAngles.y);

//         if (Input.GetKey(KeyCode.LeftShift))
//         {
//             moveSpeed = runningVelocity;
//         }
//         else
//         {
//             moveSpeed = walkingVelocity;
//         }

//         if (Input.GetKey("w"))
//         {
//             movementDirection = new Vector3(xdirection, 0.0f, zdirection);
//             characterController.Move(movementDirection * moveSpeed * Time.deltaTime);
//         }
//         if (Input.GetKey("s"))
//         {
//             movementDirection = new Vector3(xdirection, 0.0f, zdirection);
//             characterController.Move(-movementDirection * walkingVelocity * Time.deltaTime);
//         }
//         if (Input.GetKey("a"))
//         {
//             movementDirection = new Vector3(xdirection, 0.0f, zdirection);
//             movementDirection = Quaternion.Euler(0, -90, 0) * movementDirection;
//             characterController.Move(-movementDirection * walkingVelocity * Time.deltaTime * -1);
//         }
//         if (Input.GetKey("d"))
//         {
//             movementDirection = new Vector3(xdirection, 0.0f, zdirection);
//             movementDirection = Quaternion.Euler(0, 90, 0) * movementDirection;
//             characterController.Move(movementDirection * walkingVelocity * Time.deltaTime);
//         }


//         // Changes the height position of the player.
//         if (Input.GetKey(KeyCode.Space) && playerIsGrounded)
//         {
//             playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
//         }

//         playerVelocity.y += gravityValue * Time.deltaTime;
//         characterController.Move(playerVelocity * Time.deltaTime);
//     }

// }
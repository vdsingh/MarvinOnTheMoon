using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public class FPS_Player : MonoBehaviour
{
    // Camera Variables
    public float horizontalSens = 1.0f;
    public float verticalSens = 1.0f;
    private float xRotation = 0.0f;
    private float yRotation = 0.0f;

    // Components
    private CharacterController characterController;
    public GameObject monkeyBody;
    private Animator animator;
    public Camera firstPersonCamera;
    public Camera thirdPersonCamera;
    public RawImage object_icon;
    public TMP_Text object_name;
    public TMP_Text object_gravity;
    public TMP_Text gravity_text;
    public GameObject panel;
    private GameObject pause_menu;
    private LineRenderer lineRender;
    private AudioSource audioSource;
    private GameObject gg;

    // Audio Clips
    public AudioClip gun_shot_clip;



    // Player State (Variable)
    private bool playerIsGrounded;
    private Vector3 playerVelocity;
    private Vector3 movementDirection;
    private bool isCarryingObject = false;
    private GameObject objectCarried;
    private bool gravity_mode = false;

    // Player Constants
    private float range = 100.0f;
    private float walkingVelocity = 15.0f;
    private float runningVelocity = 30.0f;
    private float jumpHeight = 10.0f;
    private float gravityValue = -1.62f; // This is the moon's gravity


    // Start is called before the first frame update
    void Start()
    {
        pause_menu = GameObject.Find("Pause Menu Controller");

        firstPersonCamera.enabled = true;
        thirdPersonCamera.enabled = false;
        monkeyBody.active = false;

        gg = GameObject.Find("GGParent");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        characterController = gameObject.GetComponent<CharacterController>();

        animator = monkeyBody.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        lineRender = gameObject.AddComponent<LineRenderer>();

        Debug.Log(object_icon);
    }

    void FixedUpdate()
    {
        if (isCarryingObject)
        {
            Draw_Laser();
        }
        else
        {
            lineRender.positionCount = 0;
        }
    }

    void Draw_Laser()
    {
        lineRender.positionCount = 2;
        List<Vector3> points = new List<Vector3>();
        points.Add(GameObject.Find("GravityGunTip").transform.position);
        points.Add(objectCarried.transform.position);
        lineRender.startWidth = 0.1f;
        lineRender.endWidth = 0.1f;
        lineRender.SetPositions(points.ToArray());
        lineRender.useWorldSpace = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (pause_menu != null)
        {
            if (pause_menu.GetComponent<PauseMenu>().paused)
            {
                panel.active = false;
                return;
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            gravity_mode = !gravity_mode;
        }

        if (firstPersonCamera.enabled)
        {
            if (gravity_mode)
            {
                gravity_text.text = "Gravity Mode";
                GameObject.Find("GravityGun").GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            }
            else
            {
                gravity_text.text = "Carrying Mode";
                GameObject.Find("GravityGun").GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
            }
        }
        else
        {
            gravity_text.text = "";
        }

        Update_Camera();
        HandleMovement();
        Looking_At_Object();
        // Scroll_Wheel_Gravity();
    }

    GameObject Find_Prefab_Object(GameObject obj)
    {
        GameObject temp = obj;
        while (temp.transform.parent != null)
        {
            temp = temp.transform.parent.gameObject;
            if (temp.tag == "Prefab")
            {
                return temp;
            }
        }
        return obj;
    }

    IEnumerator Carry_Object(GameObject obj)
    {
        objectCarried = obj;
        float distance = Vector3.Distance(obj.transform.position, Camera.main.transform.position);
        while (Input.GetKey(KeyCode.Mouse0) && !gravity_mode)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                distance += 1.0f;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                distance -= 1.0f;
            }
            if (distance < 1.0f)
            {
                distance = 1.0f;
            }
            Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, range);
            if (hit.collider != null)
            {
                GameObject hit_obj = Find_Prefab_Object(hit.collider.gameObject);
                if (hit_obj != obj)
                {
                    distance = Mathf.Min(distance, Vector3.Distance(hit.point, Camera.main.transform.position) - obj.transform.localScale.x);
                }
            }
            obj.transform.position = Camera.main.transform.position + Camera.main.transform.forward * distance;
            yield return null;
        }
        obj.GetComponent<ChangableGravity>().ClearVelocity();
        isCarryingObject = false;
        yield break;
    }

    void Looking_At_Object()
    {
        RaycastHit hit;
        // if (Physics.Raycast(GetComponent<Camera>().transform.position, GetComponent<Camera>().transform.forward, out hit, range))
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range))
        {
            panel.active = true;
            GameObject obj = Find_Prefab_Object(hit.collider.gameObject);
            Texture2D icon = (Texture2D)AssetPreview.GetAssetPreview(obj);
            object_icon.texture = icon;
            object_name.text = obj.name;

            if (obj.GetComponent<ChangableGravity>() == null)
            {
                object_gravity.text = "Gravity: N/A";
                return;
            }
            object_gravity.text = "Gravity: " + obj.GetComponent<ChangableGravity>().gravity * -1.0f;
            if (gravity_mode && Input.GetKeyDown(KeyCode.Mouse0))
            {
                obj.GetComponent<ChangableGravity>().gravity -= 1;
                audioSource.clip = gun_shot_clip;
                audioSource.Play();
            }
            if (gravity_mode && Input.GetKeyDown(KeyCode.Mouse1))
            {
                obj.GetComponent<ChangableGravity>().gravity += 1;
                obj.GetComponent<ChangableGravity>().gravity = Mathf.Min(obj.GetComponent<ChangableGravity>().gravity, 0);
                audioSource.clip = gun_shot_clip;
                audioSource.Play();
            }
            bool carryable = obj.GetComponent<ChangableGravity>().isCarryable;
            if (!isCarryingObject && Input.GetKeyDown(KeyCode.Mouse0) && !gravity_mode && carryable)
            {
                isCarryingObject = true;
                StartCoroutine(Carry_Object(obj));
            }
        }
        else
        {
            panel.active = false;
        }
    }

    void Update_Camera()
    {
        //If the user clicks the ` key, switch the camera to a third person view: (Implemented by Vik)
        if (Input.GetKeyDown("`"))
        {
            firstPersonCamera.enabled = !firstPersonCamera.enabled;
            thirdPersonCamera.enabled = !thirdPersonCamera.enabled;
            if (firstPersonCamera.enabled)
            {
                monkeyBody.active = false;
                gg.active = true;
            }
            else
            {
                monkeyBody.active = true;
                gg.active = false;
            }
        }


        float mouseX = Input.GetAxis("Mouse X") * horizontalSens;
        float mouseY = Input.GetAxis("Mouse Y") * verticalSens;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);
        yRotation += mouseX;
        // GetComponent<Camera>().transform.eulerAngles = new Vector3(xRotation, yRotation, 0.0f);
        if (thirdPersonCamera.enabled)
        {
            Camera.main.transform.eulerAngles = new Vector3(xRotation, yRotation, 0.0f);
            transform.eulerAngles = new Vector3(0.0f, yRotation, 0.0f);
        }
        else
        {
            Camera.main.transform.eulerAngles = new Vector3(xRotation, yRotation, 0.0f);
            GameObject.Find("GGParent").transform.eulerAngles = new Vector3(xRotation, yRotation, 0.0f);
        }
    }

    void HandleMovement()
    {
        playerIsGrounded = characterController.isGrounded;
        float moveSpeed;

        if (playerIsGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        float xdirection = Mathf.Sin(Mathf.Deg2Rad * Camera.main.transform.rotation.eulerAngles.y);
        float zdirection = Mathf.Cos(Mathf.Deg2Rad * Camera.main.transform.rotation.eulerAngles.y);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = runningVelocity;
        }
        else
        {
            moveSpeed = walkingVelocity;
        }

        if (Input.GetKey("w"))
        {
            movementDirection = new Vector3(xdirection, 0.0f, zdirection);
            characterController.Move(movementDirection * moveSpeed * Time.deltaTime);
            animator.SetBool("walkingForward", true);
        }
        else
        {
            animator.SetBool("walkingForward", false);
        }

        if (Input.GetKey("s"))
        {
            movementDirection = new Vector3(xdirection, 0.0f, zdirection);
            characterController.Move(-movementDirection * walkingVelocity * Time.deltaTime);
            animator.SetBool("walkingBackward", true);
        } else {
            animator.SetBool("walkingBackward", false);
        }

        if (Input.GetKey("a"))
        {
            movementDirection = new Vector3(xdirection, 0.0f, zdirection);
            movementDirection = Quaternion.Euler(0, -90, 0) * movementDirection;
            characterController.Move(-movementDirection * walkingVelocity * Time.deltaTime * -1);
            animator.SetBool("walkingLeft", true);
        }
        else
        {
            animator.SetBool("walkingLeft", false);
        }

        if (Input.GetKey("d"))
        {
            movementDirection = new Vector3(xdirection, 0.0f, zdirection);
            movementDirection = Quaternion.Euler(0, 90, 0) * movementDirection;
            characterController.Move(movementDirection * walkingVelocity * Time.deltaTime);
            animator.SetBool("walkingRight", true);
        }
        else
        {
            animator.SetBool("walkingRight", false);
        }


        // Changes the height position of the player.
        if (Input.GetKey(KeyCode.Space) && playerIsGrounded)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            animator.SetBool("jumping", true);
        } else if(playerIsGrounded) {
            animator.SetBool("jumping", false);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }
}

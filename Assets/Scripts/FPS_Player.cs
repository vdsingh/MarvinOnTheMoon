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
    public Image health_bar;

    // Audio Clips
    public AudioClip gun_shot_clip;
    public AudioClip gun_beam_clip;

    // Player State (Variable)
    private bool playerIsGrounded;
    private Vector3 playerVelocity;
    private Vector3 movementDirection;
    private bool isCarryingObject = false;
    private GameObject objectCarried;
    private bool gravity_mode = false;
    private int health = 100;
    private Vector3 velocity = Vector3.zero;
    private bool hasWon = false;

    // Player Constants
    private float range = 100.0f;
    private float walkingVelocity = 15.0f;
    private float runningVelocity = 30.0f;
    private float jumpHeight = 3.0f;
    private float gravityValue = -1.62f; // This is the moon's gravity


    // Start is called before the first frame update
    void Start()
    {
        pause_menu = GameObject.Find("Pause Menu Controller");
        health_bar.color = Color.green;
        health_bar.fillAmount = 1.0f;
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
        if(hasWon) {
            return;
        }

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
        if(hasWon) {
            return;
        }

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
            Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, range);
            if (hit.collider != null)
            {
                GameObject hit_obj = Find_Prefab_Object(hit.collider.gameObject);
                if (hit_obj != obj && hit_obj.GetComponent<TextMeshPro>() == null && hit_obj.name != "Player")
                {
                    distance = Mathf.Min(distance, Vector3.Distance(hit.point, Camera.main.transform.position) - obj.transform.localScale.x);
                }
            }
            if (distance < 1.0f)
            {
                distance = 1.0f;
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
            if (obj.GetComponent<ChangableGravity>().isChangeable)
            {
                object_gravity.text += " (Changeable)";
                if (gravity_mode && Input.GetKeyDown(KeyCode.Mouse0))
                {
                    obj.GetComponent<ChangableGravity>().gravity -= 1;
                    float maxGravity = obj.GetComponent<ChangableGravity>().maxGravity;
                    obj.GetComponent<ChangableGravity>().gravity = Mathf.Max(obj.GetComponent<ChangableGravity>().gravity, maxGravity);
                    audioSource.clip = gun_shot_clip;
                    audioSource.volume = 0.1f;
                    audioSource.Play();
                }
                if (gravity_mode && Input.GetKeyDown(KeyCode.Mouse1))
                {
                    obj.GetComponent<ChangableGravity>().gravity += 1;
                    obj.GetComponent<ChangableGravity>().gravity = Mathf.Min(obj.GetComponent<ChangableGravity>().gravity, 0);
                    audioSource.clip = gun_shot_clip;
                    audioSource.volume = 0.1f;
                    audioSource.Play();
                }
            }
            else
            {

                object_gravity.text += " (Locked)";
            }
            bool carryable = obj.GetComponent<ChangableGravity>().isCarryable;
            if (!isCarryingObject && Input.GetKeyDown(KeyCode.Mouse0) && !gravity_mode && carryable)
            {
                isCarryingObject = true;
                StartCoroutine(Carry_Object(obj));
                audioSource.clip = gun_beam_clip;
                audioSource.volume = 0.6f;
                audioSource.Play();
            }
            else if (!gravity_mode && !isCarryingObject)
            {
                audioSource.Stop();
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

        playerVelocity.x = 0;
        playerVelocity.z = 0;
        if (playerIsGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        float xdirection = Mathf.Sin(Mathf.Deg2Rad * Camera.main.transform.rotation.eulerAngles.y);
        float zdirection = Mathf.Cos(Mathf.Deg2Rad * Camera.main.transform.rotation.eulerAngles.y);

        if (Input.GetKey("w"))
        {
            movementDirection = new Vector3(xdirection, 0.0f, zdirection);
            if (Input.GetKey(KeyCode.LeftShift))
            {
                playerVelocity += movementDirection * runningVelocity;
            }
            else
            {
                playerVelocity += movementDirection * walkingVelocity;
            }
            animator.SetBool("walkingForward", true);
        }
        else
        {
            animator.SetBool("walkingForward", false);
        }

        if (Input.GetKey("s"))
        {
            movementDirection = new Vector3(xdirection, 0.0f, zdirection);
            playerVelocity -= movementDirection * walkingVelocity;
            animator.SetBool("walkingBackward", true);
        }
        else
        {
            animator.SetBool("walkingBackward", false);
        }

        if (Input.GetKey("a"))
        {
            movementDirection = new Vector3(xdirection, 0.0f, zdirection);
            movementDirection = Quaternion.Euler(0, -90, 0) * movementDirection;
            playerVelocity += movementDirection * walkingVelocity;
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
            playerVelocity += movementDirection * walkingVelocity;
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
        }
        else if (playerIsGrounded)
        {
            animator.SetBool("jumping", false);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }

    internal void Damage()
    {
        health -= Random.Range(5, 10);
        health_bar.fillAmount = (float)health / 100.0f;
        if (health <= 75)
        {
            health_bar.color = Color.yellow;
        }
        if (health <= 50)
        {
            health_bar.color = Color.red;
        }
    }

    private void ResetAnimationBools() {
        animator.SetBool("walkingForward", false);
        animator.SetBool("walkingBackward", false);
        animator.SetBool("walkingLeft", false);
        animator.SetBool("walkingRight", false);
        animator.SetBool("jumping", false);
        animator.SetBool("dancing", false);
    }

    public void Victory() {
        firstPersonCamera.enabled = false;
        thirdPersonCamera.enabled = true;

        thirdPersonCamera.GetComponent<Transform>().localPosition = new Vector3(0, 4.0f, 6.0f);
        thirdPersonCamera.GetComponent<Transform>().rotation = Quaternion.Euler(20.0f, 180.0f, 0.0f);

        monkeyBody.active = true;
        gg.active = false;

        ResetAnimationBools();
        animator.SetBool("dancing", true);

        hasWon = true;

    }
}

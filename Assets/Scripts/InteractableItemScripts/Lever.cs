using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class Lever : MonoBehaviour
{
    [SerializeField] private UnityEvent leverOn;
    [SerializeField] private UnityEvent leverOff;
    public GameObject leverBox;
    public Transform pivotPoint;
    public GameObject handle;
    private bool isOn;
    private bool wasOn;
    private ChangableGravity grav;
    public float requiredGrav = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        handle.transform.RotateAround(pivotPoint.position, Vector3.left, 45);
        wasOn = false;
        isOn = false;
        grav = leverBox.GetComponent<ChangableGravity>();
        leverOff.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        if(-grav.gravity >= requiredGrav)
        {
            isOn = true;
        }
        else
        {
            isOn = false;
        }

        if(isOn && handle.transform.rotation.x <= 0.45f)
        {
            Debug.Log(handle.transform.rotation.x);
            handle.transform.RotateAround(pivotPoint.position, Vector3.right, 50 * Time.deltaTime);
        }

        if(!isOn && handle.transform.rotation.x >= -0.45f)
        {
            handle.transform.RotateAround(pivotPoint.position, Vector3.left, 50 * Time.deltaTime);
        }

        if(isOn && !wasOn)
        {
            leverOn.Invoke();
            wasOn = isOn;
        }

        if(!isOn && wasOn)
        {
            leverOff.Invoke();
            wasOn = isOn;
        }
    }

    
}

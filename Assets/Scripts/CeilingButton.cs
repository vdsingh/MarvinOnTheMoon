using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CeilingButton : MonoBehaviour
{
    [SerializeField] private UnityEvent pressed;
    [SerializeField] private UnityEvent released;
    // Start is called before the first frame update

    public GameObject pressurePlate;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter()
    {
        pressed.Invoke();
        pressurePlate.GetComponent<Renderer>().material.color = Color.green;
    }

    void OnTriggerExit()
    {
        released.Invoke();
        pressurePlate.GetComponent<Renderer>().material.color = Color.red;
    }
}

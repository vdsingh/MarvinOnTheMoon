using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    // Start is called before the first frame update
    // float weightOnPlate = 0.0f;
    float minimumWeight = 1.0f;
    float baseHeight;
    public GameObject plate;

    public GameObject gate;

    private int numObjectsOnPlate = 0;

    void Start()
    {
        Debug.Log("Pressure Plate Start");
        baseHeight = plate.transform.localScale[1];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Detect collisions between the GameObjects with Colliders attached
    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Collision detected.");
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if(rb != null) {
            Debug.Log("Collided with another RB");
            numObjectsOnPlate += 1;
            // weightOnPlate += ;

            if(rb.mass > minimumWeight) {
                Debug.Log("Weight is large enough: " + rb.mass);
                Vector3 newScale = plate.transform.localScale;
                newScale[1] = 0.5f;
                plate.transform.localScale = newScale;

                gate.GetComponent<Gate>().OpenGate();
            }
        }

        
    }

    void OnTriggerExit(Collider collision) {

        

        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if(rb != null) {
            numObjectsOnPlate -= 1;
            if(numObjectsOnPlate == 0) {
                Vector3 newScale = plate.transform.localScale;
                newScale[1] = baseHeight;
                plate.transform.localScale = newScale;
                gate.GetComponent<Gate>().CloseGate();
            }
        }

        // if(weightOnPlate < minimumWeight) {
        
    }
}

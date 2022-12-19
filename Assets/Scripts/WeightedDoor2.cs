using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedDoor2 : MonoBehaviour
{
    private ChangableGravity doorGrav;
    private ChangableGravity counterGrav;
    public GameObject door;
    public GameObject counter;
    private Rigidbody doorRb;
    private Rigidbody counterRb;

    public float changeSpeed = 0.001f;
    public float doorGravity = -1.62f;
    public float counterGravity = 0.0f;
    private float doorMass;
    private float counterMass;
    private float doorWeight;
    private float counterWeight;
    private float totalWeight;
    private float doorHeight;
    private float counterHeight;
    private bool doorStop;
    private bool counterStop;

    // Start is called before the first frame update
    void Start()
    {
        doorGrav = door.GetComponent<ChangableGravity>();
        counterGrav = counter.GetComponent<ChangableGravity>();
        doorRb = door.GetComponent<Rigidbody>();
        counterRb = counter.GetComponent<Rigidbody>();    
        doorGrav.gravity = doorGravity;
        counterGrav.gravity = counterGravity; 
        doorGrav.ignoreForce = true;
        counterGrav.ignoreForce = true;
        doorMass = doorRb.mass;
        counterMass = counterRb.mass;
        doorWeight = -doorGravity * doorMass;
        counterWeight = -counterGravity * counterMass;
        totalWeight = doorWeight + counterWeight;
        doorHeight = 9.0f - (doorWeight/totalWeight * 9.0f) + 4.5f;
        counterHeight = 9.0f - (counterWeight/totalWeight * 9.0f);
        door.transform.localPosition = new Vector3(door.transform.localPosition.x, doorHeight, door.transform.localPosition.z);
        counter.transform.localPosition = new Vector3(counter.transform.localPosition.x, counterHeight, counter.transform.localPosition.z);
        counterStop = true;
        doorStop = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        doorGravity = doorGrav.gravity;
        counterGravity = counterGrav.gravity;
        doorMass = doorRb.mass;
        counterMass = counterRb.mass;
        float newDoorWeight = -doorGravity * doorMass;
        float newCounterWeight = -counterGravity * counterMass;
        if (newDoorWeight != doorWeight || newCounterWeight != counterWeight) // Check if weights have changed
        {
            doorStop = false;
            counterStop = false;
            counterWeight = newCounterWeight;
            doorWeight = newDoorWeight;
            totalWeight = counterWeight + doorWeight;
            doorHeight = 9.0f - (doorWeight/totalWeight * 9.0f) + 4.5f;
            counterHeight = 9.0f - (counterWeight/totalWeight * 9.0f);
        }
        if(!doorStop)
        {
            if (door.transform.localPosition.y < doorHeight)
            {
                door.transform.localPosition += new Vector3(0, changeSpeed * Time.time, 0);
                if (door.transform.localPosition.y >= doorHeight)
                    doorStop = true;
            }
            else if (door.transform.localPosition.y > doorHeight)
            {
                door.transform.localPosition -= new Vector3(0, changeSpeed * Time.time, 0);
                if (door.transform.localPosition.y <= doorHeight)
                    doorStop = true;
            }
        }
        if(!counterStop)
        {
            if (counter.transform.localPosition.y < counterHeight)
            {
                counter.transform.localPosition += new Vector3(0, changeSpeed * Time.time, 0);
                if (counter.transform.localPosition.y >= counterHeight)
                    counterStop = true;
            }
            else if (counter.transform.localPosition.y > counterHeight)
            {
                counter.transform.localPosition -= new Vector3(0, changeSpeed * Time.time, 0);
                if (counter.transform.localPosition.y <= counterHeight)
                    counterStop = true;
            }
        }
    }
}

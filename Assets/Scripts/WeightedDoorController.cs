using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedDoorController : MonoBehaviour
{
    public float doorGravity = -1.62f;
    public float weightGravity = 0.0f;
    private ChangableGravity doorGrav;
    private ChangableGravity weightGrav;
    public GameObject door;
    public GameObject weight;
    private bool doorHeavier;
    private bool doorStop;
    private bool weightStop;
    private float totalGravity;
    private float doorHeight;
    private float weightHeight;
    // Start is called before the first frame update
    void Start()
    {
        doorGrav = door.GetComponent<ChangableGravity>();
        weightGrav = weight.GetComponent<ChangableGravity>();
        weightGrav.gravity = weightGravity;
        doorGrav.gravity = doorGravity;
        doorGravity = doorGrav.gravity;
        weightGravity = weightGrav.gravity;
        totalGravity = Mathf.Abs(doorGravity) + Mathf.Abs(weightGravity);
        doorHeight = Mathf.Abs((doorGravity / totalGravity) * 15.0f);
        weightHeight = Mathf.Abs((weightGravity / totalGravity) * 15.0f);
        doorStop = false;
        weightStop = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // If door is negative and weight positive or vice versa, make the heavier of the two go all the way down, lighter go all the way up. Current system only works if both are the same weight
        doorGravity = doorGrav.gravity;
        weightGravity = weightGrav.gravity;
        if (totalGravity != Mathf.Abs(doorGravity) + Mathf.Abs(weightGravity))
        {
            totalGravity = Mathf.Abs(doorGravity) + Mathf.Abs(weightGravity);
            doorHeight = Mathf.Abs((doorGravity / totalGravity) * 15.0f);
            weightHeight = Mathf.Abs((weightGravity / totalGravity) * 15.0f);
            doorStop = false;
            weightStop = false;

        }
        if(doorGravity == weightGravity)
        {
            doorHeight = 7.5f;
            weightHeight = 7.5f;
        }
        else if (doorGravity < 0 && weightGravity >= 0)
        {
            doorHeight = 15.0f;
            weightHeight = 0.0f;
        }
        else if (doorGravity >= 0 && weightGravity < 0)
        {
            doorHeight = 0.0f;
            weightHeight = 15.0f;
        }

        if (door.transform.localPosition.y < doorHeight && !doorStop)
        {
          door.transform.localPosition += new Vector3(0.0f, .001f * Time.time, 0.0f);
          if (door.transform.localPosition.y >= doorHeight)
            doorStop = true;  
        }
        
        else if (door.transform.localPosition.y > doorHeight && !doorStop)
        {
            door.transform.localPosition -= new Vector3(0.0f, .001f * Time.time, 0.0f);
            if (door.transform.localPosition.y <= doorHeight)
                doorStop = true;  
        }

        if (weight.transform.localPosition.y < weightHeight && !weightStop)
        {
            weight.transform.localPosition += new Vector3(0.0f, .001f * Time.time, 0.0f);  
            if (weight.transform.localPosition.y >= weightHeight)
                weightStop = true;
        }
        else if (weight.transform.localPosition.y > weightHeight && !weightStop)
        {
            weight.transform.localPosition -= new Vector3(0.0f, .001f * Time.time, 0.0f);
            if (weight.transform.localPosition.y <= weightHeight)
                weightStop = true;  
        }
        Debug.Log(doorHeight);
    }
}

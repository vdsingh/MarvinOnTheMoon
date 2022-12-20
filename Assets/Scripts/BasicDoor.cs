using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDoor : MonoBehaviour
{

    public GameObject door;
    public bool isOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        if(isOpen)
        {
            door.transform.localPosition = new Vector3(door.transform.localPosition.x, 7, door.transform.localPosition.z);
        }
        else
        {
             door.transform.localPosition = new Vector3(door.transform.localPosition.x, 0, door.transform.localPosition.z);
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isOpen && door.transform.localPosition.y < 7.0f)
        {
            door.transform.localPosition += new Vector3(0, 0.01f * Time.time, 0);
        }
        else
        {
            if(!isOpen && door.transform.localPosition.y > 0.0f)
            {
                door.transform.localPosition -= new Vector3(0, 0.01f * Time.time, 0);
            }
        }
    }

    public void OpenDoor()
    {
        isOpen = true;
    }

    public void CloseDoor()
    {
        isOpen = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasicDoor : MonoBehaviour
{

    public GameObject door;
    public bool isOpen = false;

    // The number of conditions that must be met for the door to open
    public bool useNumConditions = false;
    public int numConditionsToMeet = 1;
    private int numConditions = 0;
    // Start is called before the first frame update

    [SerializeField] private UnityEvent numConditionsMet;
    [SerializeField] private UnityEvent numConditionsNotMet;
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

        Debug.Log("Num conditions met: " + numConditions);
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

    public void IncrementConditions()
    {
        numConditions++;
        if(useNumConditions && numConditions >= numConditionsToMeet) {
            numConditionsMet.Invoke();
        }
    }

    public void DecrementConditions() {
        numConditions--;
        if(useNumConditions && numConditions < numConditionsToMeet) {
            numConditionsNotMet.Invoke();
        }
    }
}

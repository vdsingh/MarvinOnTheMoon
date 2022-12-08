using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{

    bool isOpen = false;

    public GameObject leftDoor;
    public GameObject rightDoor;

    private float originalDoorWidth;

    // Start is called before the first frame update
    void Start()
    {
        // originalDoorWidth = leftDoor.transform.width;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void openGate() {
        Debug.Log("Gate opening.");
        Vector3 newScale = leftDoor.transform.localScale;
        newScale[0] = 1;
        leftDoor.transform.localScale = newScale;
        rightDoor.transform.localScale = newScale;
    }

    public void closeGate() {
        Vector3 newScale = leftDoor.transform.localScale;
        newScale[0] = originalDoorWidth;
        leftDoor.transform.localScale = newScale;
        rightDoor.transform.localScale = newScale;
    }
}

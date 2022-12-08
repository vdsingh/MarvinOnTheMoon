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
        originalDoorWidth = leftDoor.transform.localScale[2];
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenGate() {
        Debug.Log("Gate opening.");
        Vector3 newScale = leftDoor.transform.localScale;
        newScale[2] = 1;
        leftDoor.transform.localScale = newScale;
        rightDoor.transform.localScale = newScale;
    }

    public void CloseGate() {
        Vector3 newScale = leftDoor.transform.localScale;
        newScale[2] = originalDoorWidth;
        leftDoor.transform.localScale = newScale;
        rightDoor.transform.localScale = newScale;
    }
}

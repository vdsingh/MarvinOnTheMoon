using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area1 : MonoBehaviour
{

    public VerticalGate exitGate;
    // Start is called before the first frame update

    void Start()
    {
        exitGate.openGate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

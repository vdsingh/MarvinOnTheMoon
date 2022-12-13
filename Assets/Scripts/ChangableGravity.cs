using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangableGravity : MonoBehaviour
{
    internal float gravity = -1.62f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Physics.gravity = new Vector3(0, gravity, 0);
    }
}

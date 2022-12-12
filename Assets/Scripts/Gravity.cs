using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    internal float gravity = -1.62f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab)) {
            gravity = -gravity;
        }
        transform.position = transform.position + new Vector3(0, gravity * Time.deltaTime, 0);
    }
}

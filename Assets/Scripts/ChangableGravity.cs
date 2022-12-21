using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class ChangableGravity : MonoBehaviour
{
    public int gravity = -1;
    private Rigidbody rb;
    internal bool ignoreForce;
    public bool isCarryable = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!ignoreForce)
        {
            rb.AddForce(0, gravity, 0);
        }
    }

    public void ClearVelocity()
    {
        rb.velocity = Vector3.zero;
    }

}

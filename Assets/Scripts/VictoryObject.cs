using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Trigger Entered");
        if(other.GetComponent<FPS_Player>() != null) {
            FPS_Player player = other.GetComponent<FPS_Player>();
            player.Victory();
            Destroy(this.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeighScale : MonoBehaviour
{
    public GameObject displayObj;
    private float totalWeight;
    private TextMeshPro displayText;
    private List<GameObject> weights;
    // Start is called before the first frame update
    void Start()
    {
        weights = new List<GameObject>();
        totalWeight = 0.0f;
        displayText = displayObj.GetComponent<TextMeshPro>();
        displayText.text = "Current Weight:\n0.00";
        
    }

    // Update is called once per frame
    void Update()
    {
        totalWeight = 0;
        foreach (var i in weights)
        {
            totalWeight += -i.GetComponent<ChangableGravity>().gravity * i.GetComponent<Rigidbody>().mass;
        }
        displayText.text = "Current Weight:\n" + totalWeight.ToString();
    }


    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<ChangableGravity>() != null)
        {
            weights.Add(other.gameObject);
            //totalWeight += -other.gameObject.GetComponent<ChangableGravity>().gravity * other.gameObject.GetComponent<Rigidbody>().mass;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.GetComponent<ChangableGravity>() != null)
        {
            weights.Remove(other.gameObject);
            //totalWeight -= -other.gameObject.GetComponent<ChangableGravity>().gravity * other.gameObject.GetComponent<Rigidbody>().mass;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class MathScale : MonoBehaviour
{
    [SerializeField] private UnityEvent rightWeight;
    [SerializeField] private UnityEvent wrongWeight;
    public GameObject displayObj;
    public float requiredWeight;
    private float totalWeight;
    private TextMeshPro displayText;
    private List<GameObject> weights;
    private bool madeWeight;
    private bool prevMade;
    // Start is called before the first frame update
    void Start()
    {
        weights = new List<GameObject>();
        totalWeight = 0.0f;
        displayText = displayObj.GetComponent<TextMeshPro>();
        displayText.text = "Required Weight:\n" + requiredWeight.ToString();
        madeWeight = false;
        prevMade = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        totalWeight = 0;
        foreach (var i in weights)
        {
            totalWeight += -i.GetComponent<ChangableGravity>().gravity * i.GetComponent<Rigidbody>().mass;
        }

        if (totalWeight == requiredWeight)
        {
            madeWeight = true;
        }
        else
        {
            madeWeight = false;
        }

        if (madeWeight && !prevMade)
        {
            rightWeight.Invoke();
            prevMade = madeWeight;
        }

        if (!madeWeight && prevMade)
        {
            wrongWeight.Invoke();
            prevMade = madeWeight;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<ChangableGravity>() != null)
        {
            weights.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.GetComponent<ChangableGravity>() != null)
        {
            weights.Remove(other.gameObject);
        }
    }
}

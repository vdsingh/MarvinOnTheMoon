using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnterLevel : MonoBehaviour
{
    [SerializeField] private UnityEvent enteredLevel;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            enteredLevel.Invoke();
        }
    }
}

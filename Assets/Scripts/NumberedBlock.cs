using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NumberedBlock : MonoBehaviour
{
    // public GameObject block;
    public GameObject face1;
    public GameObject face2;
    public GameObject face3;
    public GameObject face4;
    public GameObject face5;
    public GameObject face6;
    private ChangableGravity grav;
    private TextMeshPro text1;
    private TextMeshPro text2;
    private TextMeshPro text3;
    private TextMeshPro text4;
    private TextMeshPro text5;
    private TextMeshPro text6;

    public Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        grav = gameObject.GetComponent<ChangableGravity>();
        text1 = face1.GetComponent<TextMeshPro>();
        text2 = face2.GetComponent<TextMeshPro>();
        text3 = face3.GetComponent<TextMeshPro>();
        text4 = face4.GetComponent<TextMeshPro>();
        text5 = face5.GetComponent<TextMeshPro>();
        text6 = face6.GetComponent<TextMeshPro>();
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        string gNum = (-grav.gravity).ToString();
        text1.text = gNum;
        text2.text = gNum;  
        text3.text = gNum;  
        text4.text = gNum;  
        text5.text = gNum;  
        text6.text = gNum;
        
        gameObject.GetComponent<Renderer>().material.color = new Color(-grav.gravity/20, 1-(-grav.gravity/20), 0f, 1.0f);
    }
}

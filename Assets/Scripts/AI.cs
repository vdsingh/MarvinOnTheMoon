using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum AIState
{
    Idle,
    MovingToCube,
    MovingCube
}

public class AI : MonoBehaviour
{
    public List<GameObject> cubes;
    public bool active;
    public float distanceThreshold = 20.0f;
    private AIState state;
    private GameObject currentCube;
    private LineRenderer lineRender;
    public float distAboveCube = 3.0f;
    public Animator anim;
    private bool animation;

    // Start is called before the first frame update
    void Start()
    {
        state = AIState.Idle;
        lineRender = gameObject.AddComponent<LineRenderer>();
        anim.SetBool("Open_Anim", false);
    }

    GameObject GetCubeFarthestFromStart()
    {
        GameObject farthest = null;
        float maxDist = 0.0f;
        foreach (var i in cubes)
        {
            Vector3 startPos = i.GetComponent<NumberedBlock>().startPos;
            float dist = Vector3.Distance(i.transform.position, startPos);
            if (dist > maxDist)
            {
                maxDist = dist;
                farthest = i;
            }
        }
        if(maxDist > distanceThreshold)
            return farthest;
        else
            return null;
    }

    // Update is called once per frame
    void Update()
    {
        if(!active || animation)
            return;
        Debug.Log("State: " + state.ToString() + " Current Cube: " + currentCube + " Active: " + active);
        if(state == AIState.Idle)
        {
            GameObject farthest = GetCubeFarthestFromStart();
            if (farthest != null)
            {
                currentCube = farthest;
                state = AIState.MovingToCube;
                return;
            }
        }
        if(state == AIState.MovingToCube)
        {
            Vector3 aboveCube = currentCube.transform.position + new Vector3(0.0f, distAboveCube, 0.0f);
            float dist = Vector3.Distance(aboveCube, transform.position);
            if(dist > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, aboveCube, 0.1f);
            }
            else
            {
                state = AIState.MovingCube;
                StartCoroutine(Animate(true));
            }
        }
        if(state == AIState.MovingCube)
        {
            Draw_Laser();
            float distToStart = Vector3.Distance(currentCube.transform.position, currentCube.GetComponent<NumberedBlock>().startPos);
            float distToCube = Vector3.Distance(currentCube.transform.position, transform.position);
            if (distToCube > distAboveCube + 0.5f)
            {
                state = AIState.Idle;
                StartCoroutine(Animate(false));
                return;
            }
            if(distToStart > 0.1f)
            {
                Vector3 endPos = currentCube.GetComponent<NumberedBlock>().startPos + new Vector3(0.0f, distAboveCube, 0.0f);
                transform.position = Vector3.MoveTowards(transform.position, endPos, 0.1f);
                currentCube.transform.position = transform.position - new Vector3(0.0f, distAboveCube, 0.0f);
            }
            else
            {
                state = AIState.Idle;
                lineRender.positionCount = 0;
                StartCoroutine(Animate(false));
            }
        }
        else
        {
            lineRender.positionCount = 0;
        }
    }

    IEnumerator Animate(bool open)
    {
        animation = true;
        anim.SetBool("Open_Anim", open);
        yield return new WaitForSeconds(3.5f);
        animation = false;
    }

    void Draw_Laser()
    {
        lineRender.positionCount = 2;
        List<Vector3> points = new List<Vector3>();
        points.Add(currentCube.transform.position);
        points.Add(transform.position + new Vector3(0.0f, distAboveCube-0.5f, 0.0f));
        lineRender.startWidth = 0.1f;
        lineRender.endWidth = 0.1f;
        lineRender.SetPositions(points.ToArray());
        lineRender.useWorldSpace = true;
    }

    public void Activate()
    {
        active = true;
    }

    public void Deactivate()
    {
        active = false;
    }
}

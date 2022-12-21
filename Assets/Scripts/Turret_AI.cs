using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_AI : MonoBehaviour
{
    private GameObject player;
    public float fire_rate;
    public float proj_velocity;
    public float proj_lifespan;
    private float EPSILON, MAX_ITER;
    private Vector3 prev_player_position;

    private IEnumerator Fire()
    {
        while (true)
        {
            FireProjectile();
            yield return new WaitForSeconds(1.0f/fire_rate);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        StartCoroutine("Fire");
        EPSILON = 0.01f;
        MAX_ITER = 1000;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target_position = getDeflectionPosition(player.transform.position, player.GetComponent<CharacterController>().velocity, transform.position, proj_velocity);
        if(Mathf.Abs(target_position.x) < 1000000.0f)
        {
            transform.LookAt(target_position);
            prev_player_position = target_position;
        }
        else
        {
            transform.LookAt(prev_player_position);
        }
    }

    void FireProjectile()
    {
        GameObject projectile = Instantiate(Resources.Load("TurretProjectile")) as GameObject;
        projectile.transform.position = transform.position;
        projectile.GetComponent<Turret_Projectile>().velocity = proj_velocity;
        projectile.GetComponent<Turret_Projectile>().direction = transform.forward;
        projectile.GetComponent<Turret_Projectile>().lifespan = proj_lifespan;
    }

    private Vector3 getDeflectionPosition(Vector3 target_position, Vector3 target_velocity, Vector3 proj_origin, float proj_speed)
    {
        float t = 0.0f;
        for(int iteration = 0; iteration < MAX_ITER; ++iteration)
        {
            float old_t = t;
            t = Vector3.Distance(proj_origin, target_position + target_velocity * t) / proj_speed;
            if(Mathf.Abs(t - old_t) < EPSILON)
            {
                break;
            }
        }
        return target_position + target_velocity * t;
    } 

    public void EnableTurret()
    {
        StopCoroutine("Fire");
    }

    public void DisableTurret()
    {
        StartCoroutine("Fire");
    }

}

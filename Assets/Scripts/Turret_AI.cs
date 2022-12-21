using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_AI : MonoBehaviour
{
    private GameObject player;
    public float fire_rate;
    public float proj_velocity;
    public float proj_lifespan;

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
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform);
    }

    void FireProjectile()
    {
        GameObject projectile = Instantiate(Resources.Load("TurretProjectile")) as GameObject;
        projectile.transform.position = transform.position;
        projectile.GetComponent<Turret_Projectile>().velocity = proj_velocity;
        projectile.GetComponent<Turret_Projectile>().direction = transform.forward;
        projectile.GetComponent<Turret_Projectile>().lifespan = proj_lifespan;
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

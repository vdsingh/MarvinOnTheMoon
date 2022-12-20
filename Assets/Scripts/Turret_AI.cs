using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_AI : MonoBehaviour
{
    private GameObject player;

    private IEnumerator Fire()
    {
        while (true)
        {
            FireProjectile();
            yield return new WaitForSeconds(1.0f);
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
        projectile.GetComponent<Turret_Projectile>().velocity = 10.0f;
        projectile.GetComponent<Turret_Projectile>().direction = transform.forward;
    }
}

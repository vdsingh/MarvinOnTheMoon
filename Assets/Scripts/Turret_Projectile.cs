using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Projectile : MonoBehaviour
{
    private float birth_time;
    public float velocity;
    public Vector3 direction;
    private bool collided_with_player;
    public float lifespan;
    
    // Start is called before the first frame update
    void Start()
    {
        birth_time = Time.time;
        collided_with_player = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - birth_time > lifespan)
        {
            Destroy(gameObject);
        }
        transform.position = transform.position + velocity * direction * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player" && !collided_with_player)
        {
            collided_with_player = true;
            other.gameObject.GetComponent<FPS_Player>().Damage();
        }
        Destroy(gameObject);
    }

}

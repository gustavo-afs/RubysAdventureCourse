using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Projectile : MonoBehaviour

{
    Rigidbody2D rigidbody2d;
    public ParticleSystem bulletHitEffect;
    public AudioClip cogClip;
     

    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        EnemyController enemyController = other.collider.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            Instantiate(bulletHitEffect, rigidbody2d.gameObject.transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
            enemyController.Fix();
        }
        //we also add a debug log to know what the projectile touch
        //Debug.Log("Projectile Collision with " + other.gameObject);
        Destroy(gameObject);


    }


    // Start is called before the first frame update
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.magnitude > 1000.0f)
        {
            Destroy(gameObject);
        }
    }
}

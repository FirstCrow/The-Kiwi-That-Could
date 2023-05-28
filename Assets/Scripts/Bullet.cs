using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This script controlls bullet behaviour once the bullet has spawned in

public class Bullet : MonoBehaviour
{

    [Header("Player Varibles")]

    public float damage;
    public float speed;

    [Tooltip("How long the bullet game object will exist before being destroyed")]
    public float lifetime;


    [Header("Object References")]
    private Rigidbody2D rb;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.rotation * Vector2.up * speed;        //Sets the bullets velocity to the correct speed and direction that the mech is facing
    }

    void Update()
    {
        lifetime -= Time.deltaTime;                                    // Counts down lifetime of bullet
        if (lifetime <= 0)
        {
            Destroy(gameObject); 
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if bullet collides with destructable game object damage it
        if (collision.gameObject.TryGetComponent<DamageableEntity>(out DamageableEntity destructableComponent))  
        {
            destructableComponent.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float damage = 1f;
    public float speed;
    public float lifetime;
    private Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.rotation * Vector2.up * speed;
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject); 
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<DamageableEntity>(out DamageableEntity destructableComponent))
        {
            destructableComponent.TakeDamage(damage);
        }
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    public float speed = 1;
    private Vector2 direction;
    private Rigidbody2D rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (Time.frameCount % 100 == 0)
        {
            direction.x = Mathf.Round(Random.value * 2) - 1;
            if(direction.x == 0)
            {
                direction.y = 0;
            }
            else
            {
                direction.y = (Mathf.Round(Random.value + 1) * 2) - 3;
                direction = ConvertToIso(direction);
            }
            Debug.Log(direction);
            
            rb.velocity = direction * speed;

        }
    }

    private Vector2 ConvertToIso(Vector2 cartesian)
    {
        Vector2 screen_pos = new Vector2(cartesian.x, cartesian.y / 2.0f);
        return screen_pos;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


// This script controlls the player behaviour while in the village scene
public class PlayerController : MonoBehaviour
{
    [Header("Player Varibles")]
    public float speed;
    public float cropRange;
    private Vector2 direction;
    private Rigidbody2D rb;
    private Animator animator;
    private bool flippedSprite;
    public Transform graphics;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    
    void Update()
    {
        direction.x = Input.GetAxisRaw("Horizontal");   //Get Horizontal Inputs (A or D | Left or Right)
        direction.y = Input.GetAxisRaw("Vertical");     //Get Vertical Inputs (W or S | Up or Down)

        if (Mathf.Abs(rb.velocity.magnitude) > 0 && !animator.GetBool("isWalking"))
        {
            animator.SetBool("isWalking", true);
        }

        else if (Mathf.Abs(rb.velocity.magnitude) == 0 && animator.GetBool("isWalking"))
        {
            animator.SetBool("isWalking", false);
        }


        if (!flippedSprite && rb.velocity.x > 0)
        {
            graphics.rotation = Quaternion.Euler(0, 180, 0);
            flippedSprite = true;
        }
        else if (flippedSprite && rb.velocity.x < 0)
        {
            graphics.rotation = Quaternion.Euler(0, 0, 0);
            flippedSprite = false;
        }



    }

    private void FixedUpdate()
    {
        // Controlls player movement
        if (Mathf.Abs(direction.x) > 0 && Mathf.Abs(direction.y) > 0)        //Limits diagonal movement speed
        {
            rb.velocity = ConvertToIso(direction * speed / Mathf.Sqrt(2));
        }
        else
        {
            rb.velocity = ConvertToIso(direction * speed);
        }

    }

    // Used to convert cartesian coordinates to iso coordinates
    private Vector2 ConvertToIso(Vector2 cartesian)
    {
        Vector2 screen_pos = new Vector2(cartesian.x, cartesian.y / 2);
        return screen_pos;
    }
    // Passes the cropRange Varible
    public float GetCropRange()
    {
        return cropRange;
    }
}

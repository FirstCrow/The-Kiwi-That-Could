using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// This script controlls the player in the resource run scenes

public class MechController : MonoBehaviour
{
    [Header("Mech Varibles")]
    public float speed;
    private Vector2 direction;

    [Header("Water Variables")]
    public GameObject waterPrefab;
    private int waterAmount;
    public int maxWaterAmount = 100;
    public float waterRegenCooldown = 1f;
    private float waterRegenCooldownTimer;
    private int waterRegenSpeed = 1;
    private bool isWaterRegenerating;
    public float waterCooldown = 0.1f;
    private float waterCooldownTimer;


    [Header("Links")]
    public GameObject rotationPoint;
    public GameObject bulletSpawnPoint;
    public GameObject bullet;
    public Transform graphics;
    private Animator animator;
    private Rigidbody2D rb;
    private bool flippedSprite;

   


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        flippedSprite = false;
        graphics = transform.GetChild(0);

        waterCooldownTimer = waterCooldown;
        waterAmount = 100;
        isWaterRegenerating = false;
    }


    void Update()
    {
        direction.x = Input.GetAxisRaw("Horizontal");   //Get Horizontal Inputs (A or D | Left or Right)
        direction.y = Input.GetAxisRaw("Vertical");     //Get Vertical Inputs (W or S | Up or Down)

        // if the LMB is clicked, shoot a bullet
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            shoot();
        }

        // if the RMB is clicked, shoot water
        waterCooldownTimer -= Time.deltaTime;
        if (Input.GetKey(KeyCode.Mouse1) && waterCooldownTimer <= 0)
        {
            sprayWater();
            waterCooldownTimer = waterCooldown;
        }

       



        /*
        if (Mathf.Abs(rb.velocity.magnitude) > 0 && !animator.GetBool("isWalking"))
        {
            animator.SetBool("isWalking", true);
        }

        else if (Mathf.Abs(rb.velocity.magnitude) == 0 && animator.GetBool("isWalking"))
        {
            animator.SetBool("isWalking", false);
        }
        */

        if(!flippedSprite && rb.velocity.x > 0)
        {
            graphics.rotation = Quaternion.Euler(0, 180, 0);
            flippedSprite = true;
        }
        else if(flippedSprite && rb.velocity.x < 0)
        {
            graphics.rotation = Quaternion.Euler(0, 0, 0);
            flippedSprite = false;
        }

        

    }

    private void FixedUpdate()
    {
        // Moves the player
        if (Mathf.Abs(direction.x) > 0 && Mathf.Abs(direction.y) > 0)        //Limits diagonal movement speed
        {
            rb.velocity = ConvertToIso(direction * speed / Mathf.Sqrt(2));
        }
        else
        {
            rb.velocity = ConvertToIso(direction * speed);
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = bulletSpawnPoint.transform.position;
        Vector2 dir = mousePos - playerPos;
        float attackangle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rotationPoint.transform.rotation = Quaternion.Euler(0, 0, attackangle - 90);

        // Locks the rotation of the player to 8 directions

        /*
        if (direction.x == 0 && direction.y > 0)          // Sets rotation point up
        {
            rotationPoint.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (direction.x > 0 && direction.y > 0)       // Sets rotation point up-right
        {
            rotationPoint.transform.rotation = Quaternion.Euler(0, 0, -63.43f);
        }
        else if (direction.x > 0 && direction.y == 0)     // Sets rotation point right
        {
            rotationPoint.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (direction.x > 0 && direction.y < 0)      // Sets rotation point down-right
        {
            rotationPoint.transform.rotation = Quaternion.Euler(0, 0, -116.57f);
        }
        else if (direction.x == 0 && direction.y < 0)     // Sets rotation point down
        {
            rotationPoint.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (direction.x < 0 && direction.y < 0)      // Sets rotation point down-left
        {
            rotationPoint.transform.rotation = Quaternion.Euler(0, 0, 116.57f);
        }
        else if (direction.x < 0 && direction.y == 0)      // Sets rotation point left
        {
            rotationPoint.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (direction.x < 0 && direction.y > 0)      // Sets rotation point up-left
        {
            rotationPoint.transform.rotation = Quaternion.Euler(0, 0, 63.43f);
        }
        */


        waterRegenCooldownTimer -= Time.deltaTime;
        if (!isWaterRegenerating && waterRegenCooldownTimer <= 0)
        {
            isWaterRegenerating = true;
        }

        if (waterAmount >= maxWaterAmount)
        {
            isWaterRegenerating = false;
        }

        if (isWaterRegenerating)
        {
            regenerateWater();
        }
    }

    // Shoots a bullet
    private void shoot()
    {
        if(!PauseMenu.getGameIsPaused())
            Instantiate(bullet, bulletSpawnPoint.transform.position, rotationPoint.transform.rotation);
    }

    // Used to convert cartesian coordinates to iso coordinates
    private Vector2 ConvertToIso(Vector2 cartesian)
    {
        Vector2 screen_pos = new Vector2(cartesian.x, cartesian.y / 2);
        return screen_pos;
    }

    private void sprayWater()
    {
        if (!PauseMenu.getGameIsPaused() && waterAmount > 0)
        {
            Instantiate(waterPrefab, bulletSpawnPoint.transform.position, rotationPoint.transform.rotation);
            removeWater(2);
        }
    }

    private void removeWater(int amount)
    {
        waterAmount -= amount;
        waterRegenCooldownTimer = waterRegenCooldown;
        isWaterRegenerating = false;
        Debug.Log("Water amount: " + waterAmount);
        
    }

    private void regenerateWater()
    {
        waterAmount += waterRegenSpeed;
        Debug.Log("Water amount: " + waterAmount);
    }
}

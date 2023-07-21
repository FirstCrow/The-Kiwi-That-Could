using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class ItemPickUps : MonoBehaviour
{
    public float PickUpRadius = 0.1f;
    public InventoryItemData ItemData;
    private float magnetStrength = 0.1f;
    private float magnetAcceleration = 0.5f;
    private float maxMagnetStrength = 10f;
    public AudioSource pickupSound;

    private CircleCollider2D myCollider;

    private bool hasTarget;
    private Vector3 targetPosition;
    private Rigidbody2D rb;
    private InventoryHolder targetInventory;

    private void Awake()
    {
        myCollider = GetComponent<CircleCollider2D>();
        myCollider.isTrigger = true;
        myCollider.radius = PickUpRadius;
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var inventory = other.transform.GetComponent<InventoryHolder>();

        if (!inventory) return;

        if (inventory.InventorySystem.AddToInventory(ItemData, 1))
        {
            Instantiate(pickupSound);
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (hasTarget && targetInventory.InventorySystem.CheckAddToInventory(ItemData))
        {
            Vector2 targetDirection = (targetPosition - transform.position).normalized;
            rb.velocity = new Vector2(targetDirection.x, targetDirection.y) * magnetStrength;
            if(magnetStrength < maxMagnetStrength)
            {
                magnetStrength += magnetAcceleration;
            }
            
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
    public void SetTarget(Transform target)
    {
        targetInventory = target.GetComponent<InventoryHolder>();
        targetPosition = target.position;
        hasTarget = true;
    }
}

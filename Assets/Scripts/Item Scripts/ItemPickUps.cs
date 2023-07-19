using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class ItemPickUps : MonoBehaviour
{
    public float PickUpRadius = 0.5f;
    public InventoryItemData ItemData;
    public float magnetStrength = 2;

    private CircleCollider2D myCollider;

    private bool hasTarget;
    private Vector3 targetPosition;
    private Rigidbody2D rb;

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

            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (hasTarget)
        {
            Vector2 targetDirection = (targetPosition - transform.position).normalized;
            rb.velocity = new Vector2(targetDirection.x, targetDirection.y) * magnetStrength;
        }
    }
    public void SetTarget(Vector3 position)
    {
        targetPosition = position;
        hasTarget = true;
    }
}

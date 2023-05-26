using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Can be inherited by the player, enemies, or any object that will take damage and then be destroyed
public class DamageableEntity : MonoBehaviour
{
    [SerializeField] float health, maxHealth = 3f;

    private void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        if(health <=0)
        {
            Destroy(gameObject);
        }
    }
}

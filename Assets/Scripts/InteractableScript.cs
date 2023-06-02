using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableScript : MonoBehaviour
{
    public bool canInteract = false; //Is the player currently able to react to this interactable

    public GameObject playerRef;    //Reference to player

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
    }

    //When player enters trigger zone this sets canIntereact to true
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == playerRef)
        {
            Debug.Log("Player in interact box.");
            canInteract = true;
        }
    }

    //When player exits trigger zone this sets canIntereact to false
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == playerRef)
        {
            Debug.Log("Player out interact box.");
            canInteract = false;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableScript : MonoBehaviour
{
    bool canInteract = false; //Is the player currently able to react to this interactable

    private GameObject playerRef;    //Reference to player

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        //Interact when you press e key
        if (canInteract && Input.GetKeyDown("e"))
        {
            Debug.Log("Interact");
        }

    }

    //When player enters trigger zone this sets canIntereact to true
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == playerRef)
        {
            Debug.Log("Player in interact box.");
            canInteract = true;
        }
    }

    //When player exits trigger zone this sets canIntereact to false
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject == playerRef)
        {
            Debug.Log("Player out interact box.");
            canInteract = false;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CropScript : MonoBehaviour
{

    public GameObject plantedCrop;
    private GameObject player;
    private float playerRange;

    private SpriteRenderer rend;
    private Color originalColor;
    public Color outOfRangeColor;
    public Color inRangeColor;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerRange = FindObjectOfType<PlayerController>().GetCropRange();
        rend = GetComponent<SpriteRenderer>();
        originalColor = rend.color;
    }
 
    private void OnMouseOver()
    {
        float distanceFromPlant = Mathf.Abs((transform.position - player.transform.position).magnitude);
        if (distanceFromPlant <= playerRange)
        {
            if(Input.GetMouseButton(0))
            {
                Instantiate(plantedCrop, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            rend.color = inRangeColor;
        }
        else
            rend.color = outOfRangeColor;
    }

    private void OnMouseExit()
    {
        rend.color = originalColor;
    }
    
}

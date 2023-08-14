using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketTrigger : DialogueTrigger
{
    void Update()
    {
        if(!canInteract)
        {
            return;
        }


        if (Input.GetKeyDown(KeyCode.E))
        {
            OpenShop();
        }
    }

    private void OpenShop()
    {
        
    }


}

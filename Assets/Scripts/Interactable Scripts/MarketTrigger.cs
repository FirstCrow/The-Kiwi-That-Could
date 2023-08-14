using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketTrigger : DialogueTrigger
{
    public Canvas shopUI;
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
        shopUI.gameObject.SetActive(true);
    }

    private void CloseShop()
    {
        shopUI.gameObject.SetActive(false);
    }


}

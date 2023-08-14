using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class DialogueTrigger : InteractableScript
{
    public Dialogue dialogue;
    private bool dialogueStarted = false;

    private void Update()
    {
        if (canInteract)
        {
            if(Input.GetKeyDown("e"))
            {
                if (!dialogueStarted)
                    StartDialogue();
                else
                    ContinueDialogue();
            }
        }
    }

    public void StartDialogue()
    {
        dialogueStarted = true;
        DialogueSystemManager.current.StartDialogue(dialogue);
    }

    public void ContinueDialogue()
    {
        DialogueSystemManager.current.DisplayNextSentence();
    }
}

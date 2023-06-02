using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// This script manages ALL dialog in this scene

public class DialogueSystemManager : MonoBehaviour
{
    private TextMeshPro nameText;
    private TextMeshPro dialogueText;

    public float textDelay;

    public static DialogueSystemManager current;
    private Queue<string> sentences;

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {

        nameText = dialogue.nameText;
        dialogueText = dialogue.dialogueText;

        nameText.text = dialogue.name;
        
        

        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();

    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textDelay);
        }
    }

    public void EndDialogue()
    {
        Debug.Log("End of Dialogue");
    }
}

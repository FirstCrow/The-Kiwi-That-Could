using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string name;                 // Name of NPC
    public TextMeshPro dialogueText;
    public TextMeshPro nameText;

    [TextArea(3,10)]
    public string[] sentences;          // All of the sentences the NPC will say

 
}

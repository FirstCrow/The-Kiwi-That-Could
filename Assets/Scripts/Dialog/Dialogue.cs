using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string name;         // Name of NPC

    [TextArea(3,10)]
    public string[] sentences;  // All of the sentences the NPC will say
}

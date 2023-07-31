using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private Texture2D cursorTextureDown;

    private Vector2 cursorHotspot;
    void Start()
    {
        cursorHotspot = Vector2.zero;
        Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
    }

    private void Update()
    {
        
        if(Input.GetMouseButtonDown(0))
        {
            Cursor.SetCursor(cursorTextureDown, cursorHotspot, CursorMode.Auto);
        }
        else if(Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
        }
    }
}

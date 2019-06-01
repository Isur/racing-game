using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryScript : MonoBehaviour
{
    private KeyCode esc = KeyCode.Escape;
    void Update()
    {
        if(Input.GetKeyDown(esc)){
            Initiate.Fade("MainMenu", Color.black, 1.0f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TransitionScenes;
public class StoryScript : MonoBehaviour
{
    private KeyCode esc = KeyCode.Escape;
    void Update()
    {
        if(Input.GetAxis("Cancel") == 1){
            SceneTransition("MainMenu");
        }
    }
}

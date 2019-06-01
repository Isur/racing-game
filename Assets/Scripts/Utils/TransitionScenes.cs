using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransitionScenes
{
    private static float _multiplier = 1.0f;
    private static Color _color = Color.black;

    public static void SceneTransition(string SceneName){
        Initiate.Fade(SceneName, _color, _multiplier);
    }
}

using System.Collections;
using UnityEngine;
using static TransitionScenes;

public class MenuButton : MonoBehaviour
{
    [SerializeField]
    MenuButtonController menuButtonController;
    [SerializeField]
    Animator animator;
    [SerializeField]
    AnimatorFunctions animatorFunctions;
    [SerializeField]
    int thisIndex;
    private bool _wait = false;

    void buttonPressed(int button)
    {
        _wait = true;
        switch (button)
        {
            case 0:
                SceneTransition("MapSelector");
                break;
            case 1:
                SceneTransition("Story");
                break;
            case 2:
                Application.Quit();
                break;
        }
        StartCoroutine(test());
    }
    void Update()
    {
        if (menuButtonController.index == thisIndex)
        {
            animator.SetBool("selected", true);
            if (Input.GetAxis("Submit") == 1 && !_wait)
            {
                animator.SetBool("pressed", true);
                buttonPressed(thisIndex);
            }
            else if (animator.GetBool("pressed"))
            {
                animator.SetBool("pressed", false);
                animatorFunctions.disableOnce = true;
            }
        }
        else
        {
            animator.SetBool("selected", false);
        }
    }
    IEnumerator test()
    {
        yield return new WaitForSeconds(0.3f);
        _wait = false;
    }
}


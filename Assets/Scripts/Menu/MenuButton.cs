using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField] MenuButtonController menuButtonController;
    [SerializeField] Animator animator;
    [SerializeField] AnimatorFunctions animatorFunctions;
    [SerializeField] int thisIndex;
    private bool _wait = false;

    void buttonPressed(int button){
        _wait = true;
        switch(button){
            case 0:
                Debug.Log("PLAY");
                Initiate.Fade("TrainingScene",Color.black,1.0f);
                break;
            case 1:
                Debug.Log("STORY");
                break;
            case 2:
                Debug.Log("OPTIONS");
                break;
            case 3:
                Debug.Log("EXIT");
                break;
        }
        StartCoroutine(test());
    }
    void Update()
    {
        if(menuButtonController.index == thisIndex){
            animator.SetBool("selected", true);
            if(Input.GetAxis("Submit") == 1 && !_wait){
                animator.SetBool("pressed", true);
                buttonPressed(thisIndex);
            } else if(animator.GetBool("pressed")){
                animator.SetBool("pressed", false);
                animatorFunctions.disableOnce = true;
            }
        } else {
            animator.SetBool("selected", false);
        }
    }
    IEnumerator test(){
        yield return new WaitForSeconds(0.3f);
        _wait = false;
    }
}


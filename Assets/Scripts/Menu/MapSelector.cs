using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSelector : MonoBehaviour
{
    public KeyCode  nextMap = KeyCode.RightArrow,
                    prevMap = KeyCode.LeftArrow,
                    submit = KeyCode.Return,
                    esc = KeyCode.Escape;
    [SerializeField]
    Animation animationClick;
    [SerializeField]
    int thisIndex;
    

    public AudioSource Audio;
    public AudioClip ClickSound;
    public Image Slider;
    public List<Image> RaceImages = new List<Image>();
    public List<string> RaceNames = new List<string>();

    private bool _wait = false;
    private int _currentScene = 0;

    void Start() {
        Slider.sprite = RaceImages[0].sprite;   
    }
    void Update()
    {
        if(!_wait && Input.GetKeyDown(nextMap)){
            ChangeMap(1);
        } else if(!_wait && Input.GetKeyDown(prevMap)){
            ChangeMap(0);
        } else if (Input.GetKeyDown(submit)){
            Initiate.Fade(RaceNames[_currentScene], Color.black, 1.0f);
        } else if (Input.GetKeyDown(esc)){
            Initiate.Fade("MainMenu", Color.black, 1.0f);
        }
    }

    private void ChangeMap(int dir){
         _wait = true;
         Audio.PlayOneShot(ClickSound);
        if(thisIndex == dir) animationClick.Play("Click");
        if(dir == 1){
            if(_currentScene + 1 < RaceNames.Count){
                _currentScene++;
            } else {
                _currentScene = 0;
            }
        } else if (dir == 0){
            if(_currentScene > 0){
                _currentScene--;
            } else {
                _currentScene = RaceNames.Count - 1;
            }
        }
        Slider.sprite = RaceImages[_currentScene].sprite;
        StartCoroutine(buttonClick());
    }

    IEnumerator buttonClick(){
        yield return new WaitForSeconds(0.3f);
        _wait = false;
    }
}

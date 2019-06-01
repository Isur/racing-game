using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static TransitionScenes;

public class MapSelector: MonoBehaviour{
    public List<Sprite> RaceImages = new List<Sprite>();
    public List<string> RaceSceneNames = new List<string>();
    public MapSelectorButton ButtonRight;
    public MapSelectorButton ButtonLeft;

    private Image Slider;
    private bool _wait = false;
    private int _selectedRace = 0;
    private void Start() {
        if(RaceImages.Count != RaceSceneNames.Count){
            Debug.Log("You have to provide image and name for each race.");
            return;
        }
        Slider = GetComponent<Image>();
        Slider.sprite = RaceImages[_selectedRace];
    }
    
    private void Update() {
        float horizontal = Input.GetAxis("Horizontal");
        if(!_wait && horizontal != 0){
            ChangeMap(horizontal);    
        } else if (Input.GetAxis("Submit") == 1 && !_wait){
            SceneTransition(RaceSceneNames[_selectedRace]);
        } else if (Input.GetAxis("Cancel") == 1 && !_wait){
            SceneTransition("MainMenu");
        }
    }

    private void ChangeMap(float dir){
        int count = RaceSceneNames.Count;
        _wait = true;
        if(dir < 0){
            ButtonLeft.click();
            if(_selectedRace > 0){
                _selectedRace--;
            } else {
                _selectedRace = count - 1;
            }
        } else if (dir > 0){
            ButtonRight.click();
            if(_selectedRace + 1 < count){
                _selectedRace++;
            } else {
                _selectedRace = 0;
            }
        }
        StartCoroutine(Unblock());
        Slider.sprite = RaceImages[_selectedRace];
    }

    IEnumerator Unblock(){
        yield return new WaitForSeconds(0.6f);
        _wait = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSelectorButton : MonoBehaviour
{
    public void click(){
        GetComponent<Animation>().Play();
        GetComponent<AudioSource>().Play();
    }


}

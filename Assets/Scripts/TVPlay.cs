using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TVPlay : MonoBehaviour
{
    public GameObject screen;
    public VideoPlayer videoPlayer;
    public bool isPlaying = false;

    public void Play()
    {
       isPlaying = true;
       screen.SetActive(true); 
       videoPlayer.Play();
    }

}

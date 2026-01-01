using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundTrigger : MonoBehaviour
{
    public AudioSource aud;
    public TVPlay TV; //the TV object that plays the sound
    public TriggerPanel triggerPanel; //panel to remind player to press E to play sound

    private bool isInside = false;
    private bool isPlaying = false;

    private void Start()
    {
        triggerPanel = TriggerPanel.instance;
    }


    void Update()
    {
        if (aud != null && isInside && !aud.isPlaying && Input.GetKeyDown(KeyCode.E))
        {
            aud.Play();
            isPlaying = true;
            triggerPanel.Hide();
        }

        if (TV != null && isInside && !TV.isPlaying && Input.GetKeyDown(KeyCode.E))
        {
            TV.Play();
            isPlaying = true;
            triggerPanel.Hide();
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered sound trigger");
            isInside = true;
            if (isPlaying) return;
            triggerPanel.Show();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited sound trigger");
            isInside = false;
            triggerPanel.Hide();
        }
    }

}

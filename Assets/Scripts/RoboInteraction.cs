using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoboInteraction : MonoBehaviour
{
    // Start is called before the first frame update
    //public GameObject target;
    //public Dialogue dialogue;
    //public GameObject explanationText;
    //private bool Talking = false;
    private AudioSource sound;
    public AudioClip audioClip;
    private bool audioStarted = false;



    private void Start()
    {
        sound = this.GetComponent<AudioSource>();
        sound.clip = audioClip;
    }

    private void OnTriggerEnter(Collider other)

    {
        if (audioStarted) return;
        sound.Play();
        Debug.Log("AudioStart");
        audioStarted = true;
    }
    
}

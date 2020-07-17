using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip drawing;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
    }

    
    public void drawingSound()
    {
        audioSource.clip = drawing;
        audioSource.Play();
    }

    public void stopSound()
    {
        audioSource.Stop();
    }

}

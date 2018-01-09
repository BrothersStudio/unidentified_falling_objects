using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSounds : MonoBehaviour
{
    bool played_sound = false;
    AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            Debug.Log("Play ground sound");
            played_sound = true;
        }
        else if (other.tag == "Block")
        {
            Debug.Log("Play block sound");
            played_sound = true;
        }
    }
}

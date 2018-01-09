using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCollision : MonoBehaviour
{
    AudioSource source;

    CameraShake cam_shake;

    private void Start()
    {
        source = GetComponent<AudioSource>();

        cam_shake = Camera.main.GetComponent<CameraShake>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            Debug.Log("Play ground sound");
            cam_shake.ShakeCamera(0.2f);
        }
        else if (other.tag == "Block")
        {
            Debug.Log("Play block sound");
        }
    }
}

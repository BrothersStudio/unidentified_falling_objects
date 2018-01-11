using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCollision : MonoBehaviour
{
    AudioSource source;
    public AudioClip ground_sound;
    public AudioClip block_sound;

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
            source.clip = ground_sound;
            source.Play();

            cam_shake.ShakeCamera(0.2f);
        }
        else if (other.tag == "Block")
        {
            source.clip = block_sound;
            source.Play();
        }
    }
}

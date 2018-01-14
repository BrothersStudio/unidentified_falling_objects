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
            //source.volume = GetComponentInParent<Rigidbody>().velocity.magnitude / 9f;
            source.clip = ground_sound;
            source.Play();

            cam_shake.ShakeCamera(0.15f);
        }
        else if (other.tag == "Block")
        {
            //source.volume = GetComponentInParent<Rigidbody>().velocity.magnitude / 9f;
            source.clip = block_sound;
            source.Play();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCollision : MonoBehaviour
{
    AudioSource source;
    public AudioClip ground_sound;
    public AudioClip block_sound;
    int sounds_i_made = 0;
    int max_sounds = 4;

    CameraShake cam_shake;

    PlacementChecker checker;

    private void Start()
    {
        source = GetComponent<AudioSource>();

        cam_shake = Camera.main.GetComponent<CameraShake>();

        checker = GameObject.Find("PlacementChecker").GetComponent<PlacementChecker>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            if (sounds_i_made < max_sounds)
            {
                sounds_i_made++;
                source.clip = ground_sound;
                source.Play();
            }

            checker.BlockHitGround(+1);

            if (!checker.IsLevelOver())
            {
                cam_shake.ShakeCamera(0.15f);
            }
        }
        else if (other.tag == "Block")
        {
            if (sounds_i_made < max_sounds)
            {
                sounds_i_made++;
                source.clip = block_sound;
                source.Play();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ground")
        {
            checker.BlockHitGround(-1);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOController : MonoBehaviour
{
    float left_bound = -5.8f;
    float right_bound = 5.8f;
    float init_vel;
    float velocity = 0.1f;
    float accel = 0.05f;
    bool deccel_flag = false;

    bool moving_left = false;

    private void Start()
    {
        init_vel = velocity;
    }

    private void Update ()
    {
        // Check if we need to start slowing down
        if (moving_left && transform.position.x < 0 && !deccel_flag)
        {
            deccel_flag = true;
            accel = -accel;
        }
        else if (!moving_left && transform.position.x > 0 && !deccel_flag)
        {
            deccel_flag = true;
            accel = -accel;
        }

        // Apply velocity to ufo
        Vector3 new_position = transform.position;
        if (moving_left)
        {
            velocity = Mathf.Clamp(velocity + (Time.deltaTime * accel), -0.2f, -0.05f);
        }
        else
        {
            velocity = Mathf.Clamp(velocity + (Time.deltaTime * accel), 0.05f, 0.2f);
        }
        new_position.x += velocity;
        transform.position = new_position;

        // Check if we want to switch directions
        if (moving_left && transform.position.x < left_bound)
        {
            SwitchDirections();
        }
        else if (!moving_left && transform.position.x > right_bound)
        {
            SwitchDirections();
        }
        Debug.Log(velocity);
	}

    private void SwitchDirections()
    {
        deccel_flag = false;
        moving_left = !moving_left;
        if (moving_left)
        {
            velocity = init_vel;
        }
        else
        {
            velocity = -init_vel;
        }
    }
}

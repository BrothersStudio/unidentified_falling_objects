using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour
{
    // Movement variables
    float left_bound = -5.8f;
    float right_bound = 5.8f;
    bool moving_left = false;
    float init_vel;
    float velocity = 0.1f;
    float accel = 0.05f;
    bool deccel_flag = false;

    // Block variables
    public GameObject block_prefab;
    float drop_time = 0f;
    float auto_spawn_time = 3f;

    // Judgement variables
    PlacementChecker checker;

    private void Start()
    {
        init_vel = velocity;

        checker = GameObject.Find("PlacementChecker").GetComponent<PlacementChecker>();
    }

    private void Update()
    {
        if ((Input.GetKey(KeyCode.DownArrow) ||
            Input.GetKey(KeyCode.Mouse0) ||
            Input.GetKey(KeyCode.Space)) &&
            !checker.IsLevelOver())
        {
            if (transform.childCount > 0)
            {
                GetComponentInChildren<Block>().Fall();
                transform.DetachChildren();
                drop_time = Time.timeSinceLevelLoad;
            }
        }

        if (transform.childCount == 0)
        {
            if (drop_time + auto_spawn_time < Time.timeSinceLevelLoad)
            {
                SpawnNewBlock();
            }
        }
    }

    public void SpawnNewBlock()
    {
        // Judge placement of last block
        checker.CheckActiveBlock();

        if (transform.childCount == 0)
        {
            Instantiate(block_prefab, transform);
        }
    }

    private void FixedUpdate ()
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

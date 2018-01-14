﻿using System.Collections;
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
    float drop_time = 0f;
    float auto_spawn_time = 3f;

    // Judgement variables
    PlacementChecker checker;

    // Flying away variables
    float end_time;
    bool up_part = false;

    // Canvas variables
    public GameObject main_menu_text;
    public GameObject reset_text;

    private void Start()
    {
        init_vel = velocity;

        checker = GameObject.Find("PlacementChecker").GetComponent<PlacementChecker>();

        // Spawn first block
        Instantiate(GetComponent<BlockQueue>().GetNextBlock(), transform);
    }

    private void Update()
    {
        if ((Input.GetKey(KeyCode.DownArrow) ||
            Input.GetKey(KeyCode.Mouse0) ||
            Input.GetKey(KeyCode.Space)) &&
            !checker.IsLevelOver())
        {
            if (GetComponentInChildren<Block>() != null)
            {
                GetComponentInChildren<Block>().Fall();
                GetComponentInChildren<Block>().gameObject.transform.parent = null;
                drop_time = Time.timeSinceLevelLoad;
            }
        }

        if (GetComponentInChildren<Block>() == null)
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

        if (GetComponentInChildren<Block>() == null)
        {
            GameObject next_block = GetComponent<BlockQueue>().GetNextBlock();
            if (next_block != null)
            {
                Instantiate(next_block, transform);
            }
        }
    }

    private void FixedUpdate ()
    {
        if (!checker.IsLevelOver())
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
        // If the game is over, start the fly away animation
        else
        {
            if (GetComponent<Rigidbody>() == null)
            {
                end_time = Time.timeSinceLevelLoad;

                gameObject.AddComponent<Rigidbody>();
                GetComponent<Rigidbody>().useGravity = false;
            }

            if ((Time.timeSinceLevelLoad < end_time + 0.5f) && !up_part)
            {
                GetComponent<Rigidbody>().AddForce(new Vector3(0f, -4f), ForceMode.Acceleration);
            }
            else
            {
                up_part = true;
            }

            if (up_part)
            {
                GetComponent<Rigidbody>().AddForce(new Vector3(5f, 5f, 2f), ForceMode.Acceleration);
            }

            if (!GetComponentInChildren<Renderer>().isVisible)
            {
                main_menu_text.SetActive(true);
                reset_text.SetActive(true);
            }
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

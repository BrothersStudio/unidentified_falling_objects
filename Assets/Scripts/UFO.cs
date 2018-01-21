using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour
{
    // Movement variables
    public float left_bound = -5.8f;
    bool moving_left = false;
    float init_vel;
    float velocity = 0.1f;
    float accel = 0.05f;
    bool deccel_flag = false;

    float goal_height = 0f;
    float goal_height_buffer = 4.6f;
    float up_speed = 0.8f;

    // Sound variables
    AudioSource source;
    public AudioClip drop_clip;

    // Block variables
    float drop_time = 0f;
    float auto_spawn_time = 2.2f;

    // Judgement variables
    PlacementChecker checker;

    // Flying away variables
    float end_time;
    bool up_part = false;

    // Infinite mode
    public bool infinite_mode;

    private void Start()
    {
        init_vel = velocity;

        checker = GameObject.Find("PlacementChecker").GetComponent<PlacementChecker>();

        source = GetComponent<AudioSource>();

        // Spawn first block
        Instantiate(HandoffNextBlock(GetComponent<BlockQueue>().GetNextBlock()), transform);
    }

    private void Update()
    {
        if (!checker.IsLevelOver())
        {
            if ((Input.GetKey(KeyCode.DownArrow) ||
                Input.GetKey(KeyCode.Mouse0) ||
                Input.GetKey(KeyCode.Space)) &&
                (Time.timeSinceLevelLoad > 0.5f))
            {
                if (GetComponentInChildren<Block>() != null)
                {
                    source.clip = drop_clip;
                    source.Play();

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
    }

    public void SpawnNewBlock()
    {
        // Judge placement of last block
        goal_height = checker.CheckActiveBlock();

        if (GetComponentInChildren<Block>() == null)
        {
            GameObject next_block = HandoffNextBlock(GetComponent<BlockQueue>().GetNextBlock());
            if (next_block != null && !checker.IsLevelOver())
            {
                Instantiate(next_block, transform);
            }
        }
    }

    private GameObject HandoffNextBlock(NextBlocks next)
    {
        if (next == null)
        {
            return null;
        }

        if (infinite_mode)
        {
            checker.DisplayNextBlock(next.after);
        }
        return next.next;
    }

# region MOVEMENT
    private void FixedUpdate ()
    {
        if (!checker.IsLevelOver())
        {
            Vector3 new_position = transform.position;

            // Fly upward if we need to get away from the last placed block
            if (transform.position.y < goal_height + goal_height_buffer)
            {
                new_position.y += up_speed * Time.deltaTime;
            }

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
            else if (!moving_left && transform.position.x > -left_bound)
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
                checker.UFOIsOffScreen();
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
#endregion

}

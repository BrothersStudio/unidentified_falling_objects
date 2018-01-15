using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUFO : MonoBehaviour
{
    float speed = 2f;
    float sine_place_y = 0f;
    float sine_speed_y = 0.08f;
    float sine_amp_y = 2.5f;
    float sine_place_z = 0f;
    float sine_speed_z = 0.04f;
    float sine_amp_z = 1f;
    Vector3 starting_position;

	void Start ()
    {
        starting_position = transform.localPosition;
	}
	
	void FixedUpdate ()
    {
        Vector3 new_position = transform.localPosition;

        new_position.x += Time.deltaTime * speed;
        new_position.y += Time.deltaTime * Mathf.Sin(sine_place_y) * sine_amp_y;
        sine_place_y += sine_speed_y;
        new_position.z += Time.deltaTime * Mathf.Sin(sine_place_z) * sine_amp_z;
        sine_place_z += sine_speed_z;

        transform.localPosition = new_position;

        if (new_position.x > 7.5)
        {
            Vector3 new_start = starting_position;
            new_start.y += Random.Range(-1f, 1f);
            transform.localPosition = new_start;
        }
    }
}

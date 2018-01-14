using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUFO : MonoBehaviour
{
    float speed = 4f;
    float sine_place = 0f;
    float sine_speed = 0.08f;
    float sine_amp = 2.5f;
    Vector3 starting_position;

	void Start ()
    {
        starting_position = transform.localPosition;
	}
	
	void FixedUpdate ()
    {
        Vector3 new_position = transform.localPosition;

        new_position.x += Time.deltaTime * speed;
        new_position.y += Time.deltaTime * Mathf.Sin(sine_place) * sine_amp;
        sine_place += sine_speed;

        transform.localPosition = new_position;

        if (!GetComponentInChildren<Renderer>().isVisible)
        {
            Vector3 new_start = starting_position;
            new_start.y += Random.Range(-1f, 1f);
            transform.localPosition = new_start;
        }
    }
}

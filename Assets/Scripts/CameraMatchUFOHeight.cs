using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMatchUFOHeight : MonoBehaviour
{
    Transform ufo;
    PlacementChecker checker;
    Vector3 offset;

    bool back_to_start = false;
    Vector3 original_position;
    Vector3 last_position;

    private void Start()
    {
        ufo = GameObject.Find("UFO").transform;
        checker = GameObject.Find("PlacementChecker").GetComponent<PlacementChecker>();

        offset = transform.position - ufo.position;
        original_position = transform.position;
    }

    private void LateUpdate()
    {
        if (!checker.end_screen)
        {
            Vector3 new_position = ufo.position + offset;
            new_position.x = transform.position.x;

            transform.position = new_position;
            last_position = new_position;
        }
        else if (!back_to_start)
        {
            transform.position = last_position;
            back_to_start = true;
        }

        if (back_to_start)
        {
            transform.position = Vector3.MoveTowards(transform.position, original_position, 0.1f);
        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMatchUFOHeight : MonoBehaviour
{
    Transform ufo;
    PlacementChecker checker;
    Vector3 offset;

    private void Start()
    {
        ufo = GameObject.Find("UFO").transform;
        checker = GameObject.Find("PlacementChecker").GetComponent<PlacementChecker>();

        offset = transform.position - ufo.position;
    }

    private void LateUpdate()
    {
        Vector3 new_position = ufo.position + offset;
        new_position.x = transform.position.x;

        transform.position = new_position;

        if (checker.IsLevelOver())
        {
            Invoke("Suicide", 3f);
        }
	}

    private void Suicide()
    {
        Destroy(this);
    }
}

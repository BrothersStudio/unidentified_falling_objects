using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTurnAndFace : MonoBehaviour
{
    public GameObject title;
    public GameObject start_button;
    public GameObject left_button;
    public GameObject right_button;

    public Transform title_card;
    public Transform level1;
    public Transform level2;
    Transform current_target;

    Vector3 direction;
    Quaternion lookRotation;

    float turn_speed = 4f;

    private void Start()
    {
        current_target = title_card;
    }

    private void FixedUpdate ()
    {
        direction = (current_target.position - transform.position).normalized;

        lookRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turn_speed);
	}

    public void StartGame()
    {
        ToggleTitle();
        current_target = level1;
    }

    public void ChangeLevels(bool left)
    {
        if (current_target == level1)
        {
            current_target = level2;
        }
        else
        {
            current_target = level1;
        }
    }

    private void ToggleTitle()
    {
        title.SetActive(false);
        start_button.SetActive(false);
        left_button.SetActive(true);
        right_button.SetActive(true);
    }
}

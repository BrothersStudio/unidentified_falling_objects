using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTurnAndFace : MonoBehaviour
{
    public GameObject title;
    public GameObject start_button;

    public Transform title_card;
    public Transform level1;
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

    public void Title()
    {
        current_target = title_card;
    }

    public void Level1()
    {
        current_target = level1;
        ToggleTitle();
    }

    private void ToggleTitle()
    {
        title.SetActive(!title.activeSelf);
        start_button.SetActive(!start_button.activeSelf);
    }
}

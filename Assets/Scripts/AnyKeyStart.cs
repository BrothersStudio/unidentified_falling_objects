using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyKeyStart : MonoBehaviour
{
    CameraTurnAndFace menu;

    private void Start()
    {
        menu = Camera.main.GetComponent<CameraTurnAndFace>();
    }

    private void Update ()
    {
        if (Input.anyKey)
        {
            menu.StartGame();
            gameObject.SetActive(false);
        }
    }
}

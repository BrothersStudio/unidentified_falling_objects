using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
	void Update ()
    {
        if (Input.GetKey(KeyCode.Escape) && Time.timeSinceLevelLoad > 1f)
        {
            //Application.Quit();
        }
    }
}

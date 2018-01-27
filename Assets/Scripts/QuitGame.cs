using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitGame : MonoBehaviour
{
	void Update ()
    {
        if (Input.GetKey(KeyCode.Escape) && Time.timeSinceLevelLoad > 3f && SceneManager.sceneCount == 1)
        {
            Application.Quit();
        }
    }
}

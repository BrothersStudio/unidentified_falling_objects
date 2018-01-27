using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    string current_scene;
    private bool loading = false;

    private void Start()
    {
        current_scene = SceneManager.GetActiveScene().name;
    }

    public void ReturnToMainMenu()
    {
        loading = true;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
    }

    private void Update()
    {
        // Wait while main menu is loading before unloading current scene
        if (loading)
        {
            GameObject[] main_objects = SceneManager.GetSceneByName("MainMenu").GetRootGameObjects();
            for (int i = 0; i < main_objects.Length; i++)
            {
                Camera.main.GetComponent<AudioListener>().enabled = false;

                if (main_objects[i].name == "Main Menu Camera")
                {
                    main_objects[i].GetComponent<CameraTurnAndFace>().SelectLevel(current_scene);
                    break;
                }
            }
            SceneManager.UnloadSceneAsync(current_scene);
        }

        if (Input.GetKey(KeyCode.R))
        {
            RestartLevel();
        }

        if (Input.GetKey(KeyCode.Escape) && !loading)
        {
            GameObject.Find("Canvas/Return").GetComponent<Button>().onClick.Invoke();
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

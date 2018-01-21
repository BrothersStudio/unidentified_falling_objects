using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
        loading = true;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            QuitGame();
        }

        if (Input.GetKey(KeyCode.R) && current_scene != "MainMenu")
        {
            RestartLevel();
        }

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
    }

    public void RestartLevel()
    {
        int scene_ind = SceneIndices.GetIndex(SceneManager.GetActiveScene().name);
        LeaderboardDriver.PerformCreateOperation(scene_ind, 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

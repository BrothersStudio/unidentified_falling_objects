using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    string current_scene;
    private bool loading = false;

    public void ReturnToMainMenu()
    {
        current_scene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
        loading = true;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            QuitGame();
        }

        // Wait while main menu is loading before unloading current scene
        if (loading)
        {
            GameObject[] main_objects = SceneManager.GetSceneByName("MainMenu").GetRootGameObjects();
            for (int i = 0; i < main_objects.Length; i++)
            {
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

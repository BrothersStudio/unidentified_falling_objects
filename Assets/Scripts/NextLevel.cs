using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public void NextScene()
    {
        string current_name = SceneManager.GetActiveScene().name;
        int next_ind = SceneIndices.GetIndex(current_name) + 1;
        string next_name = SceneIndices.GetName(next_ind);
        SceneManager.LoadScene(next_name);
    }
}

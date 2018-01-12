using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraTurnAndFace : MonoBehaviour
{
    public GameObject title;
    public GameObject start_button;
    public GameObject left_button;
    public GameObject right_button;
    public GameObject play_button;
    public GameObject score_button;
    public GameObject pyramids_title;
    public GameObject stonehenge_title;

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

        LoadScores();
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
        SelectLevel("Pyramids");
    }

    public void ChangeLevels(bool left)
    {
        if (current_target == level1)
        {
            SelectLevel("Stonehenge");
        }
        else
        {
            SelectLevel("Pyramids");
        }
    }

    public void SelectLevel(string selected)
    {
        switch (selected)
        {
            case "Pyramids":
                current_target = level1;
                pyramids_title.SetActive(true);
                stonehenge_title.SetActive(false);
                SetLevelScores(selected);
                break;
            case "Stonehenge":
                current_target = level2;
                stonehenge_title.SetActive(true);
                pyramids_title.SetActive(false);
                SetLevelScores(selected);
                break;
        }
    }

    public void PlayLevel()
    {
        if (current_target == level1)
        {
            SceneManager.LoadScene("Pyramids");
        }
        else if (current_target == level2)
        {
            SceneManager.LoadScene("Stonehenge");
        }
    }

    private void ToggleTitle()
    {
        title.SetActive(false);
        start_button.SetActive(false);
        left_button.SetActive(true);
        right_button.SetActive(true);
        play_button.SetActive(true);
        score_button.SetActive(true);
    }

    private void SetLevelScores(string selected)
    {

    }

    private void LoadScores()
    {

    }
}

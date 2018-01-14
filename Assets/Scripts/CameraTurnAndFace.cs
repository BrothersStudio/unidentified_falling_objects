using Amazon.DynamoDBv2.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CameraTurnAndFace : MonoBehaviour
{
    public GameObject title;
    public GameObject start_button;
    public GameObject name_panel;

    public GameObject left_button;
    public GameObject right_button;
    public GameObject play_button;
    public GameObject score_button;
    public GameObject score_table;
    public GameObject pyramids_title;
    public GameObject stonehenge_title;
    public GameObject eiffel_title;
    public GameObject pisa_title;

    public Transform title_card;
    public Transform level1;
    public Transform level2;
    public Transform level3;
    public Transform level4;
    Transform current_target;

    Vector3 direction;
    Quaternion lookRotation;

    float turn_speed = 4f;

    // Scores
    private int current_level = 0;
    private List<Dictionary<string, AttributeValue>> scores;

    private void Start()
    {
        current_target = title_card;
    }

    private void Update()
    {
        if (LeaderboardDriver.Readable)
        {
            scores = LeaderboardDriver.Results;

            string score_string = "";
            Text score_text = score_table.transform.Find("Scores").GetComponent<Text>();
            foreach (var item in scores)
            {
                score_string += item["Score"].N + " " + item["Name"].S + '\n';
            }
            score_text.text = score_string;
        }
    }

    private void FixedUpdate ()
    {
        direction = (current_target.position - transform.position).normalized;

        lookRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turn_speed);
	}

    public void StartGame()
    {
        if (LeaderboardDriver.Name == null)
        {
            name_panel.SetActive(true);
        }
        else
        {
            ToggleTitle();
            SelectLevel("Pyramids");
        }
    }

    public void ChangeLevels(bool left)
    {
        if (left)
        {
            if (current_target == level1)
            {
                SelectLevel("Pisa");
            }
            else if (current_target == level2)
            {
                SelectLevel("Pyramids");
            }
            else if (current_target == level3)
            {
                SelectLevel("Stonehenge");
            }
            else if (current_target == level4)
            {
                SelectLevel("EiffelTower");
            }
        }
        else // Right
        {
            if (current_target == level1)
            {
                SelectLevel("Stonehenge");
            }
            else if (current_target == level2)
            {
                SelectLevel("EiffelTower");
            }
            else if (current_target == level3)
            {
                SelectLevel("Pisa");
            }
            else if (current_target == level4)
            {
                SelectLevel("Pyramids");
            }
        }
    }

    public void SelectLevel(string selected)
    {
        switch (selected)
        {
            case "Pyramids":
                current_level = 1;
                current_target = level1;
                pyramids_title.SetActive(true);
                stonehenge_title.SetActive(false);
                eiffel_title.SetActive(false);
                pisa_title.SetActive(false);
                break;
            case "Stonehenge":
                current_level = 2;
                current_target = level2;
                stonehenge_title.SetActive(true);
                pyramids_title.SetActive(false);
                eiffel_title.SetActive(false);
                pisa_title.SetActive(false);
                break;
            case "EiffelTower":
                current_level = 3;
                current_target = level3;
                eiffel_title.SetActive(true);
                pyramids_title.SetActive(false);
                stonehenge_title.SetActive(false);
                pisa_title.SetActive(false);
                break;
            case "Pisa":
                current_level = 4;
                current_target = level4;
                pisa_title.SetActive(true);
                eiffel_title.SetActive(false);
                pyramids_title.SetActive(false);
                stonehenge_title.SetActive(false);
                break;
        }
        LeaderboardDriver.FindScoresForLevel(current_level);
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
        else if (current_target == level3)
        {
            SceneManager.LoadScene("EiffelTower");
        }
        else if (current_target == level4)
        {
            SceneManager.LoadScene("Pisa");
        }
    }

    private void ToggleTitle()
    {
        title.SetActive(false);
        start_button.SetActive(false);
        name_panel.SetActive(false);

        left_button.SetActive(true);
        right_button.SetActive(true);
        play_button.SetActive(true);
        score_button.SetActive(true);
    }
}

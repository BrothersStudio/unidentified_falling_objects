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
    public GameObject score_prefab;
    Transform score_holder;
    public GameObject loading_text;

    private void Start()
    {
        current_target = title_card;

        score_holder = score_table.transform.Find("Score Holder");
    }

    private void Update()
    {
        if (LeaderboardDriver.Readable && (score_holder.childCount == 0))
        {
            loading_text.SetActive(false);

            scores = LeaderboardDriver.Results;

            foreach (var item in scores)
            {
                GameObject score = Instantiate(score_prefab, score_holder);
                score.transform.Find("Name").GetComponent<Text>().text = item["Name"].S;
                score.transform.Find("Score").GetComponent<Text>().text = item["Score"].N;

                // If we see the current user's score, change the color.
                if (item["Id"].S == LeaderboardDriver.Id)
                {
                    Color current_color = score.GetComponent<Image>().color;
                    current_color.a = 1;
                    score.GetComponent<Image>().color = current_color;
                }
            }
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
        ToggleTitle();

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
        ClearLeaderboard();
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
        score_table.SetActive(true);
    }

    private void ClearLeaderboard()
    {
        foreach (Transform child in score_holder)
        {
            Destroy(child.gameObject);
        }
        loading_text.SetActive(true);
    }
}

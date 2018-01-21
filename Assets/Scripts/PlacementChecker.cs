using Amazon.DynamoDBv2.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlacementChecker : MonoBehaviour
{
    GameObject[] ghost_blocks;
    bool level_done = false;

    // Score
    int blocks_dropped = 0;
    int score = 0;
    float score_factor = 1000;
    public Text score_text;
    public GameObject score_word_prefab;
    public GameObject canvas;
    bool score_sent = false;

    // Sounds
    public AudioClip perfect_sound;
    public AudioClip great_sound;
    public AudioClip good_sound;
    public AudioClip miss_sound;

    // Canvas
    public GameObject reset_text;
    public GameObject main_menu_button;
    public GameObject reset_button;
    public GameObject end_game_menu;
    public GameObject leaderboard;

    public bool end_screen = false;
    private bool name_on_board = false;
    private Transform score_holder;
    public GameObject score_prefab;
    public ParticleSystem fireworks;

    // Infinite
    bool infinite_mode = false;
    int grounded_blocks = 0;
    int grounded_limit = 4;
    public Text ground_counter;
    GameObject visible_next_block;
    public Transform next_block_location;

    private void Start()
    {
        ghost_blocks = GameObject.FindGameObjectsWithTag("Ghost");

        infinite_mode = GameObject.Find("UFO").GetComponent<UFO>().infinite_mode;

        if (infinite_mode)
        {
            ground_counter.text = "Allowed on Ground: " + (grounded_limit - 1).ToString();
        }

        score_holder = leaderboard.transform.Find("Score Holder");
        LeaderboardDriver.FindScoresForLevel(SceneIndices.GetIndex(SceneManager.GetActiveScene().name));
    }

    private void Update()
    {
        // Send score if not sent yet and level is done
        if (IsLevelOver() && !score_sent)
        {
            int scene_ind = SceneIndices.GetIndex(SceneManager.GetActiveScene().name);
            LeaderboardDriver.PerformCreateOperation(scene_ind, score);

            score_sent = true;
        }

        // Read scoreboard scores as necessary
        if (LeaderboardDriver.Readable && IsLevelOver() && score_holder.childCount == 0)
        {
            leaderboard.transform.Find("Loading...").gameObject.SetActive(false);

            List<Dictionary<string, AttributeValue>> scores = LeaderboardDriver.Results;

            foreach (var item in scores)
            {
                if (score_holder.transform.childCount < 15)
                {
                    string score_string = item["Score"].N;
                    int score_int = int.Parse(score_string);

                    if (score > score_int && !name_on_board)
                    {
                        name_on_board = true;
                        GameObject user_score_object = Instantiate(score_prefab, score_holder);
                        user_score_object.transform.Find("Name").GetComponent<Text>().text = LeaderboardDriver.Name;
                        user_score_object.transform.Find("Score").GetComponent<Text>().text = score.ToString();

                        Color current_color = user_score_object.GetComponent<Image>().color;
                        current_color.a = 1;
                        user_score_object.GetComponent<Image>().color = current_color;
                    }

                    GameObject score_object = Instantiate(score_prefab, score_holder);
                    score_object.transform.Find("Name").GetComponent<Text>().text = item["Name"].S;
                    score_object.transform.Find("Score").GetComponent<Text>().text = score_string;

                    // If we see the current user's score, check if the new one is higher
                    if (item["Id"].S == LeaderboardDriver.Id)
                    {
                        if (name_on_board)
                        {
                            Destroy(score_object);
                        }
                        else
                        {
                            name_on_board = true;
                            Color current_color = score_object.GetComponent<Image>().color;
                            current_color.a = 1;
                            score_object.GetComponent<Image>().color = current_color;
                        }
                    }
                }
            }

            if (scores.Count == 0)
            {
                name_on_board = true;
                GameObject user_score_object = Instantiate(score_prefab, score_holder);
                user_score_object.transform.Find("Name").GetComponent<Text>().text = LeaderboardDriver.Name;
                user_score_object.transform.Find("Score").GetComponent<Text>().text = score.ToString();

                Color current_color = user_score_object.GetComponent<Image>().color;
                current_color.a = 1;
                user_score_object.GetComponent<Image>().color = current_color;
            }
        }

        if (infinite_mode)
        {
            if (grounded_blocks >= grounded_limit)
            {
                level_done = true;
                fireworks.Play();
            }
        }
    }

    public void DisplayNextBlock(GameObject next_block)
    {
        Destroy(visible_next_block);
        visible_next_block = Instantiate(next_block, next_block_location.position, Quaternion.identity);
        visible_next_block.AddComponent<Rigidbody>();
        visible_next_block.GetComponent<BoxCollider>().enabled = true;
        visible_next_block.GetComponentInChildren<BoxCollision>().for_show = true;
        visible_next_block.tag = "Untagged";
    }

    // Called when UFO spawns in new block. Judges placement of last block.
    public float CheckActiveBlock()
    {
        if (!infinite_mode)
        {
            GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
            for (int i = 0; i < blocks.Length; i++)
            {
                if (blocks[i].GetComponent<Block>().IsActiveBlock())
                {
                    // Find closest ghost block to active block
                    float min_dist = Mathf.Infinity;
                    GameObject min_block = null;
                    for (int j = 0; j < ghost_blocks.Length; j++)
                    {
                        if (ghost_blocks[j].activeSelf)
                        {
                            float this_dist = Vector3.Distance(blocks[i].transform.position, ghost_blocks[j].transform.position);
                            if (this_dist < min_dist)
                            {
                                min_dist = this_dist;
                                min_block = ghost_blocks[j];
                            }
                        }
                    }

                    // Set closest ghost to dropped block as inactive
                    if (min_block != null)
                    {
                        blocks_dropped++;
                        float added_score = CalculateDistanceScore(min_dist);
                        SpawnScoreWord(added_score, blocks[i].transform.position);

                        // Check for pity reset text
                        if (blocks_dropped > 1 && (score / (score_factor * blocks_dropped) < 0.3))
                        {
                            reset_text.SetActive(true);
                        }

                        min_block.SetActive(false);
                        blocks[i].GetComponent<Block>().SetInactiveBlock();

                        CalculateLevelOver();
                        return CheckLastBlockHeight(blocks[i]);
                    }
                }
            }
        }
        else  // Infinite mode logic
        {
            GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
            for (int i = 0; i < blocks.Length; i++)
            {
                if (blocks[i].GetComponent<Block>().IsActiveBlock())
                {
                    blocks[i].GetComponent<Block>().SetInactiveBlock();

                    float block_height = CheckLastBlockHeight(blocks[i]);
                    CalculateTotalHeightScore(block_height);
                    return block_height;
                }
            }
        }

        Debug.LogWarning("Wasn't able to calculate new UFO height don't ignore this");
        return 0;
    }

    public void BlockHitGround(int hit)
    {
        grounded_blocks += hit;

        if (infinite_mode)
        {
            ground_counter.text = "Allowed on Ground: " + Mathf.Clamp((grounded_limit - grounded_blocks - 1), 0, 100).ToString();
        }
    }

    private float CheckLastBlockHeight(GameObject block)
    {
        float block_height_from_center = block.GetComponent<MeshFilter>().mesh.bounds.extents.y * block.transform.localScale.y;
        float block_y_pos = block.transform.position.y;

        return block_y_pos + block_height_from_center;
    }

    private float CalculateDistanceScore(float dist)
    {
        float added_score = (score_factor / Mathf.Pow((dist + 1), 2));
        score += (int)added_score;
        score_text.text = score.ToString();
        return added_score;
    }

    private void CalculateTotalHeightScore(float height)
    {
        float added_score = height * score_factor;
        if (added_score > score)
        {
            score = (int)added_score;
            score_text.text = score.ToString();
        }
    }

    private void SpawnScoreWord(float score, Vector3 spawn_location)
    {
        // Spawn and place score word over placed block
        Vector2 viewportPoint = Camera.main.WorldToViewportPoint(spawn_location);
        GameObject word = Instantiate(score_word_prefab, viewportPoint, Quaternion.identity, canvas.transform);
        word.GetComponent<RectTransform>().anchorMin = viewportPoint;
        word.GetComponent<RectTransform>().anchorMax = viewportPoint;

        // Change color and word of score word as appropriate 
        if ((score / score_factor) * 100 >= 80)
        {
            word.GetComponent<Text>().text = "Perfect!";
            word.GetComponent<Text>().color = Color.magenta;

            word.GetComponent<AudioSource>().clip = perfect_sound;
            word.GetComponent<AudioSource>().Play();
        }
        else if ((score / score_factor) * 100 >= 50)
        {
            word.GetComponent<Text>().text = "Great!";
            word.GetComponent<Text>().color = Color.green;

            word.GetComponent<AudioSource>().clip = great_sound;
            word.GetComponent<AudioSource>().Play();
        }
        else if ((score / score_factor) * 100 >= 25)
        {
            word.GetComponent<Text>().text = "Good!";
            word.GetComponent<Text>().color = Color.cyan;

            word.GetComponent<AudioSource>().clip = good_sound;
            word.GetComponent<AudioSource>().Play();
        }
        else 
        {
            word.GetComponent<Text>().text = "Miss!";
            word.GetComponent<Text>().color = Color.red;

            word.GetComponent<AudioSource>().clip = miss_sound;
            word.GetComponent<AudioSource>().Play();
        }
    }

    public void CalculateLevelOver()
    {
        // Are any ghost blocks still active?
        for (int i = 0; i < ghost_blocks.Length; i++)
        {
            if (ghost_blocks[i].activeSelf)
            {
                return;
            }
        }
        level_done = true;
        fireworks.Play();
    }

    public bool IsLevelOver()
    {
        return level_done;
    }

    public void UFOIsOffScreen()
    {
        end_screen = true;

        // Turn off buttons from when level is being played
        reset_button.SetActive(false);
        reset_text.SetActive(false);
        score_text.gameObject.SetActive(false);
        main_menu_button.SetActive(false);

        // Turn on and execute end game menus
        end_game_menu.transform.Find("Score").GetComponent<Text>().text = score.ToString();
        end_game_menu.SetActive(true);
        leaderboard.SetActive(true);
    }
}

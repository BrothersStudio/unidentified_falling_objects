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

    // Canvas
    public GameObject reset_text;

    // Infinite
    bool infinite_mode = false;
    int grounded_blocks = 0;
    int grounded_limit = 4;
    public Text ground_counter;

    private void Start()
    {
        ghost_blocks = GameObject.FindGameObjectsWithTag("Ghost");

        infinite_mode = GameObject.Find("UFO").GetComponent<UFO>().infinite_mode;

        if (infinite_mode)
        {
            ground_counter.text = "Allowed on Ground: " + (grounded_limit).ToString();
        }
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

        if (infinite_mode)
        {
            if (grounded_blocks >= grounded_limit)
            {
                level_done = true;
            }
        }
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
            ground_counter.text = "Allowed on Ground: " + Mathf.Clamp((grounded_limit - grounded_blocks), 0, 100).ToString();
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
            word.GetComponent<Text>().text = "Bad!";
            word.GetComponent<Text>().color = Color.red;
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
    }

    public bool IsLevelOver()
    {
        return level_done;
    }
}

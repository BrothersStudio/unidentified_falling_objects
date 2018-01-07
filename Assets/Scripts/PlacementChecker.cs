using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlacementChecker : MonoBehaviour
{
    GameObject[] ghost_blocks;
    bool level_done = false;

    // Score
    int score = 0;
    float score_factor = 1000;
    public Text score_text;
    public GameObject score_word_prefab;
    public GameObject canvas;

    private void Start()
    {
        ghost_blocks = GameObject.FindGameObjectsWithTag("Ghost");
    }

    // Called when UFO spawns in new block. Judges placement of last block.
    public void CheckActiveBlock()
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
                    float added_score = CalculateScore(min_dist);
                    SpawnScoreWord(added_score, blocks[i].transform.position);

                    min_block.SetActive(false);
                    blocks[i].GetComponent<Block>().SetInactiveBlock();
                    CalculateLevelOver();
                    break;
                }
            }
        }
    }

    private float CalculateScore(float added_score)
    {
        added_score = (score_factor / (added_score + 1));
        score += (int)added_score;
        score_text.text = score.ToString();
        return added_score;
    }

    private void SpawnScoreWord(float score, Vector3 spawn_location)
    {
        // Spawn and place score word over placed block
        Vector2 viewportPoint = Camera.main.WorldToViewportPoint(spawn_location);
        GameObject word = Instantiate(score_word_prefab, viewportPoint, Quaternion.identity, canvas.transform);
        word.GetComponent<RectTransform>().anchorMin = viewportPoint;
        word.GetComponent<RectTransform>().anchorMax = viewportPoint;

        // Change color and word of score word as appropriate 
        if ((score / score_factor) * 100 >= 90)
        {
            word.GetComponent<Text>().text = "Perfect!";
            word.GetComponent<Text>().color = Color.magenta;
        }
        else if ((score / score_factor) * 100 >= 70)
        {
            word.GetComponent<Text>().text = "Great!";
            word.GetComponent<Text>().color = Color.green;
        }
        else if ((score / score_factor) * 100 >= 50)
        {
            word.GetComponent<Text>().text = "Good!";
            word.GetComponent<Text>().color = Color.cyan;
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

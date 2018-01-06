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
    public Text score_text;

    private void Start()
    {
        ghost_blocks = GameObject.FindGameObjectsWithTag("Ghost");
    }

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
                    CalculateScore(min_dist);

                    min_block.SetActive(false);
                    blocks[i].GetComponent<Block>().SetInactiveBlock();
                    CalculateLevelOver();
                    break;
                }
            }
        }
    }

    private void CalculateScore(float added_score)
    {
        added_score = (1000 / (added_score + 1));
        score += (int)added_score;
        score_text.text = score.ToString();
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

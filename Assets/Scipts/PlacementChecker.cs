﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementChecker : MonoBehaviour
{
    GameObject[] ghost_blocks;

    private void Start()
    {
        ghost_blocks = GameObject.FindGameObjectsWithTag("Ghost");
    }

    public void CheckActiveBlock()
    {
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
        for (int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i].GetComponent<BlockController>().IsActiveBlock())
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
                min_block.SetActive(false);
                blocks[i].GetComponent<BlockController>().SetInactiveBlock();
                break;
            }
        }
    }

    private bool IsLevelOver()
    {
        // Are any ghost blocks still active?
        for (int i = 0; i < ghost_blocks.Length; i++)
        {
            if (ghost_blocks[i].activeSelf)
            {
                return true;
            }
        }
        return false;
    }
}

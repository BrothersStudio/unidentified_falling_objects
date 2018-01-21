using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockQueue : MonoBehaviour
{
    private int block_ind = -1;
    public List<int> blocks;
    private List<int> random_block_list = new List<int>();

    public GameObject block_small;
    public GameObject block_tall;
    public GameObject block_wide;
    public GameObject sphere;

    private void Awake()
    {
        if (GetComponentInParent<UFO>().infinite_mode)
        {
            random_block_list.Add(2);
            for (int i = 0; i < 1000; i++)
            {
                random_block_list.Add(Random.Range(0, 3));
            }
        }
    }

    public NextBlocks GetNextBlock()
    {
        block_ind++;
        if (GetComponentInParent<UFO>().infinite_mode)
        {
            NextBlocks next = new NextBlocks(BlockSwitch(random_block_list[block_ind]), BlockSwitch(random_block_list[block_ind + 1]));
            return next;
        }
        else if (block_ind < blocks.Count)
        {
            NextBlocks next = new NextBlocks(BlockSwitch(blocks[block_ind]));
            return next;
        }
        else
        {
            return null;
        }
    }

    private GameObject BlockSwitch(int reference)
    {
        switch (reference)
        {
            case 0:
                return block_small;
            case 1:
                return block_tall;
            case 2:
                return block_wide;
            case 3:
                return sphere;
        }
        return new GameObject();
    }
}

public class NextBlocks
{
    public GameObject next;
    public GameObject after;

    public NextBlocks(GameObject block1)
    {
        next = block1;
        after = null;
    }

    public NextBlocks(GameObject block1, GameObject block2)
    {
        next = block1;
        after = block2;
    }
}
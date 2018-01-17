using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockQueue : MonoBehaviour
{
    private int block_ind = -1;
    public List<int> blocks;

    public GameObject block_small;
    public GameObject block_tall;
    public GameObject block_wide;
    public GameObject sphere;

    public GameObject GetNextBlock()
    {
        block_ind++;
        if (block_ind < blocks.Count)
        {
            return BlockSwitch(blocks[block_ind]);
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

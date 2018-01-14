using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    bool detached;
    bool spawned_new_block = false;

    public void Fall()
    {
        gameObject.AddComponent<Rigidbody>();
    }

    private void Update()
    {
        // Checking to see if it's stopped moving
        if (GetComponent<Rigidbody>() != null)
        {
            if (GetComponent<Rigidbody>().velocity.y > 0)
            {
                detached = true;
            }

            if (detached && !spawned_new_block && GetComponent<Rigidbody>().velocity.y == 0)
            {
                GameObject.Find("UFO").GetComponent<UFO>().SpawnNewBlock();
            }
        }
    }

    public bool IsActiveBlock()
    {
        if (!spawned_new_block)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetInactiveBlock()
    {
        spawned_new_block = true;

        GetComponent<Rigidbody>().mass = 3f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    bool detached;
    bool spawned_new_block = false;

    public void Fall()
    {
        gameObject.AddComponent<Rigidbody>();
    }

    private void Update()
    {
        if (GetComponent<Rigidbody>() != null)
        {
            if (GetComponent<Rigidbody>().velocity.y > 0)
            {
                detached = true;
            }

            if (detached && !spawned_new_block && GetComponent<Rigidbody>().velocity.y == 0)
            {
                GameObject.Find("UFO").GetComponent<UFOController>().SpawnNewBlock();
                spawned_new_block = true;
            }
        }
    }
}

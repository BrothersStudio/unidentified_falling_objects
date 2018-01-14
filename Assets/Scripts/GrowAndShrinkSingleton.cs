using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowAndShrinkSingleton : MonoBehaviour
{
    private static GrowAndShrinkSingleton _instance;
    public static GrowAndShrinkSingleton Instance
    {
        get
        {
            return _instance;
        }
    }

    private static float scale = 1f;
    float min_scale = 0.9f;
    float max_scale = 1.1f;
    float grow_speed = 0.01f;
    bool growing = true;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            //Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void FixedUpdate()
    {
        if (growing && scale < max_scale)
        {
            scale += grow_speed;
        }
        else if (!growing && scale > min_scale)
        {
            scale -= grow_speed;
        }

        if (growing && scale >= max_scale)
        {
            growing = false;
        }
        else if (!growing && scale <= min_scale)
        {
            growing = true;
        }
    }

    public static float Scale
    {
        get
        {
            return scale;
        }
    }
}
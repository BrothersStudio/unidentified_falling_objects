using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextGrowAndShrink : MonoBehaviour
{
    float min_scale = 0.9f;
    float max_scale = 1.1f;
    float grow_speed = 0.01f;
    bool growing = true;

	void Update ()
    {
        Vector3 scale = GetComponent<RectTransform>().localScale;
        if (growing && scale.x < max_scale)
        {
            scale.x += grow_speed;
            scale.y += grow_speed;
        }
        else if (!growing && scale.x > min_scale)
        {
            scale.x -= grow_speed;
            scale.y -= grow_speed;
        }
        GetComponent<RectTransform>().localScale = scale;

        if (growing && scale.x >= max_scale)
        {
            growing = false;
        }
        else if (!growing && scale.x <= min_scale)
        {
            growing = true;
        }
    }
}

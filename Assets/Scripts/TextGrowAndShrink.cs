using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextGrowAndShrink : MonoBehaviour
{
    void FixedUpdate ()
    {
        Vector3 scale = GetComponent<RectTransform>().localScale;

        scale.x = GrowAndShrinkSingleton.Scale;
        scale.y = GrowAndShrinkSingleton.Scale;

        GetComponent<RectTransform>().localScale = scale;
    }
}

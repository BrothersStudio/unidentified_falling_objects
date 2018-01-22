using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyTitle : MonoBehaviour
{
    bool fading = false;
    float fade_speed = 0.02f;

	void Start ()
    {
        Invoke("Fading", 3f);
	}
	
	void Fading ()
    {
        fading = true;
	}

    private void Update()
    {
        if (fading)
        {
            Color current_color = GetComponent<Text>().color;
            current_color.a -= fade_speed;
            GetComponent<Text>().color = current_color;
            if (current_color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}

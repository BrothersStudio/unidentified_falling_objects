using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activity : MonoBehaviour
{
    public void Flip()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}

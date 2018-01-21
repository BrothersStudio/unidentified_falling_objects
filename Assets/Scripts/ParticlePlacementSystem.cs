using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePlacementSystem : MonoBehaviour
{
    public GameObject particlePrefab;

    void OnTriggerEnter(Collider other)
    {
        Instantiate(particlePrefab, other.gameObject.transform.position, Quaternion.identity);
    }
}

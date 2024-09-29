using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    // Time in seconds before the object is destroyed
    public float destructionTime = 5.0f;

    void Start()
    {
        // Schedule the destruction of the GameObject
        Destroy(gameObject, destructionTime);
    }
}

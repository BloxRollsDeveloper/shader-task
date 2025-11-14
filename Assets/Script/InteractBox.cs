using System;
using UnityEngine;

public class InteractBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        print("Interacting with " + other.name);
    }
}

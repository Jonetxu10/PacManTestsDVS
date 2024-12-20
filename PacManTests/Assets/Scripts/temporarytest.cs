using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temporarytest : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger entered");
        // If the player passes through the checkpoint, we activate it
        if (collision.tag == "Player")
        {
            Debug.Log("Player enters checkpoint");
        }
    }
}

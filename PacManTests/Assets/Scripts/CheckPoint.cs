using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckPoint : MonoBehaviour 
{
    public bool Activated = false;

    private SpriteRenderer thisSpriteRenderer;

    public static List<GameObject> CheckPointsList;

    void Start()
    {
        thisSpriteRenderer = GetComponent<SpriteRenderer>();
        thisSpriteRenderer.color = Color.magenta;
        Debug.Log("Should be magenta");
        // We search all the checkpoints in the current scene
        CheckPointsList = GameObject.FindGameObjectsWithTag("CheckPoint").ToList();
        Debug.Log(CheckPointsList);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger entered");
        // If the player passes through the checkpoint, we activate it
        if (collision.tag == "Player")
        {
            Debug.Log("Player enters checkpoint");
            ActivateCheckPoint();
        }
    }

    private void ActivateCheckPoint()
    {
        // We deactive all checkpoints in the scene
        foreach (GameObject cp in CheckPointsList)
        {
            cp.GetComponent<CheckPoint>().Activated = false;
            cp.GetComponent<SpriteRenderer>().color = Color.magenta;
        }

        // We activated the current checkpoint
        Activated = true;
        thisSpriteRenderer.color = Color.green;
    }

    public static Vector3 GetActiveCheckPointPosition()
    {
        // If player die without activate any checkpoint, we will return a default position
        Vector3 result = new Vector3(0, -9.5f, -5f);

        if (CheckPointsList != null)
        {
            foreach (GameObject cp in CheckPointsList)
            {
                // We search the activated checkpoint to get its position
                if (cp.GetComponent<CheckPoint>().Activated)
                {
                    result = cp.transform.position;
                    break;
                }
            }
        }

        return result;
    }
}

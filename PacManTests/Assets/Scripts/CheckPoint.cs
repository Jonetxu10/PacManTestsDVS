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
        if (CheckPointsList == null)
        {
            CheckPointsList = new List<GameObject>();
        }

        if (!CheckPointsList.Contains(gameObject))
        {
            CheckPointsList.Add(gameObject);
        }

        thisSpriteRenderer = GetComponent<SpriteRenderer>();
        thisSpriteRenderer.color = Color.magenta;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
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

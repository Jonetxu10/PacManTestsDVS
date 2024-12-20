using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/* NOMBRE CLASE: CheckPoint
 * AUTOR: Jone Sainz Egea
 * FECHA: 19/12/2024
 * VERSIÓN: 1.0 programa base con la funcionalidad
 *              1.1. se añade output visual con el spriteRenderer
 * DESCRIPCIÓN: - Genera la lista de checkpoints a partir de los objetos de la escena que contienen este script
 *              - Activa el último checkpoint por el que el jugador ha pasado y desactiva el resto
 *              - Devuelve la posición del checkpoint activo
 */
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

    /* NOMBRE MÉTODO: ActivateCheckPoint
     * AUTOR: Jone Sainz Egea
     * FECHA: 19/12/2024
     * DESCRIPCIÓN: - Desactiva todos los checkpoints de la escena
     *              - Activa el checkpoint por el que el jugador acaba de pasar
     *              - Cambia el color del checkpoint activo
     * @param: -
     * @return: -
     */
    private void ActivateCheckPoint()
    {
        foreach (GameObject cp in CheckPointsList)
        {
            cp.GetComponent<CheckPoint>().Activated = false;
            cp.GetComponent<SpriteRenderer>().color = Color.magenta;
        }

        Activated = true;
        thisSpriteRenderer.color = Color.green;
    }

    /* NOMBRE MÉTODO: GetActiveCheckPointPosition
     * AUTOR: Jone Sainz Egea
     * FECHA: 19/12/2024
     * DESCRIPCIÓN: - Establece una posición base por si no se ha activado ningún checkpoint
     *              - Busca el checkpoint activo para devolver su posición
     * @param: -
     * @return: Vector3 --> Active Checkpoint Position
     */
    public static Vector3 GetActiveCheckPointPosition()
    {
        Vector3 result = new Vector3(0, -9.5f, -5f);

        if (CheckPointsList != null)
        {
            foreach (GameObject cp in CheckPointsList)
            {
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

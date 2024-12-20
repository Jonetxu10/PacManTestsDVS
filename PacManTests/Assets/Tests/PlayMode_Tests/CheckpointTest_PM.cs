using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/* NOMBRE CLASE: CheckpointTest_PM
 * AUTOR: Jone Sainz Egea
 * FECHA: 19/12/2024
 * VERSIÓN: 1.0 programa base con la funcionalidad completa
 * DESCRIPCIÓN: Clase que se encarga de testear todo lo relacionado con los Checkpoints
 *                  - Comprueba que el checkpoint se crea
 *                  - Comprueba que el checkpoint se activa
 *                  - Comprueba que el jugador aparece en el checkpoint activo al morir
 */
public class CheckpointTest_PM
{
    private GameObject pacmanGameObject;
    private GameObject ghostGameObject;
    private GameObject cpGameObject;
    private CheckPoint newCheckPoint;

    [SetUp]
    public void Setup()
    {
        pacmanGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Pacman"), new Vector3(0, 0, 0), Quaternion.identity);
        ghostGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Ghost_Base"), new Vector3(20, 0, 0), Quaternion.identity);

        cpGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Checkpoint"), new Vector3(50, 0, 0), Quaternion.identity);
        newCheckPoint = cpGameObject.GetComponent<CheckPoint>();

        if (CheckPoint.CheckPointsList == null)
        {
            CheckPoint.CheckPointsList = new List<GameObject>();
        }
    }

    [TearDown]
    public void TearDown()
    {
        GameObject.Destroy(pacmanGameObject);
        GameObject.Destroy(ghostGameObject);
        GameObject.Destroy(cpGameObject);

        CheckPoint.CheckPointsList.Clear();
    }

    [UnityTest]
    public IEnumerator CheckPointIsCreated()
    {
        yield return null;
        Assert.AreEqual(1, CheckPoint.CheckPointsList.Count, "Se ha creado más de un checkpoint.");
        Assert.IsFalse(newCheckPoint.Activated, "Checkpoint está activo al inicio.");
    }

    [UnityTest]
    public IEnumerator CheckPointIsActivated()
    {
        pacmanGameObject.transform.position = cpGameObject.transform.position;
        yield return new WaitForSeconds(0.1f);

        Assert.IsTrue(newCheckPoint.Activated, "Checkpoint no está activo después de que Pacman pase por él.");
        Assert.AreEqual(Color.green, newCheckPoint.GetComponent<SpriteRenderer>().color, "El color del checkpoint no es verde después de activarse.");
    }

    [UnityTest]
    public IEnumerator PlayerAppearsInCheckPointActivated()
    {
        pacmanGameObject.transform.position = cpGameObject.transform.position;
        yield return new WaitForSeconds(0.1f);
        Assert.IsTrue(newCheckPoint.Activated, "Checkpoint no está activo después de que Pacman pase por él.");
        
        pacmanGameObject.GetComponent<Pacman>().ResetState(); 
        yield return null; 
        Vector3 pacmanPosition = pacmanGameObject.transform.position;
        Vector3 checkpointPosition = newCheckPoint.transform.position;

        Assert.AreEqual(checkpointPosition, pacmanPosition, "Pacman no reaparece en la posición del checkpoint activo.");
    }
}

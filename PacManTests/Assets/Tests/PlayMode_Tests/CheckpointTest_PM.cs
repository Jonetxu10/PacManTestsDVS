using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CheckpointTest_PM
{
    private GameObject pacman;
    private GameObject ghost;
    private GameObject cp;
    private CheckPoint newCheckPoint;

    [SetUp]
    public void Setup()
    {
        // Instanciar prefabs requeridos
        pacman = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Pacman"), new Vector3(0, 0, 0), Quaternion.identity);
        ghost = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Ghost_Base"), new Vector3(20, 0, 0), Quaternion.identity);

        // Instanciar checkpoint y verificar su componente
        cp = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Checkpoint"), new Vector3(50, 0, 0), Quaternion.identity);
        Assert.IsNotNull(cp, "No se pudo cargar el prefab Checkpoint.");
        newCheckPoint = cp.GetComponent<CheckPoint>();
        Assert.IsNotNull(newCheckPoint, "El objeto Checkpoint no tiene el componente CheckPoint.");

        // Inicializar lista de checkpoints si es necesario
        if (CheckPoint.CheckPointsList == null)
        {
            CheckPoint.CheckPointsList = new List<GameObject>();
        }
    }

    [TearDown]
    public void TearDown()
    {
        GameObject.Destroy(pacman);
        GameObject.Destroy(ghost);
        GameObject.Destroy(cp);

        // Limpiar lista de checkpoints
        CheckPoint.CheckPointsList.Clear();
    }

    [UnityTest]
    public IEnumerator CheckPointIsCreated()
    {
        // Verificar que se haya agregado un checkpoint a la lista
        yield return null; // Esperar un frame para inicializaci�n
        Assert.AreEqual(1, CheckPoint.CheckPointsList.Count, "Solo 1 CheckPoint deber�a estar creado.");
        Assert.IsFalse(newCheckPoint.Activated, "Checkpoint no deber�a estar activo al inicio.");
    }

    [UnityTest]
    public IEnumerator CheckPointIsActivated()
    {
        // Mover Pacman al checkpoint
        pacman.transform.position = cp.transform.position;

        // Esperar un frame para procesar la colisi�n
        yield return new WaitForSeconds(0.1f);

        // Validar activaci�n del checkpoint
        Assert.IsTrue(newCheckPoint.Activated, "Checkpoint deber�a estar activo despu�s de que Pacman pase por �l.");
        Assert.AreEqual(Color.green, newCheckPoint.GetComponent<SpriteRenderer>().color, "El color del checkpoint deber�a ser verde despu�s de activarse.");
    }

    [UnityTest]
    public IEnumerator PlayerAppearsInCheckPointActivated()
    {
        // Activar un checkpoint
        pacman.transform.position = new Vector3(50, 0, 0); // Simular que Pacman pasa por el checkpoint
        yield return new WaitForSeconds(0.1f); // Esperar que el evento de activaci�n se procese

        Assert.IsTrue(newCheckPoint.Activated, "Checkpoint deber�a estar activo despu�s de que Pacman pase por �l."); // Verificar que se activa

        // Llamar a ResetState y verificar que Pacman reaparece en el checkpoint
        pacman.GetComponent<Pacman>().ResetState(); // Llamar a ResetState
        yield return null; // Esperar al siguiente frame

        Vector3 pacmanPosition = pacman.transform.position;
        Vector3 checkpointPosition = newCheckPoint.transform.position;

        // Verificar que Pacman reaparece exactamente en el checkpoint activo
        Assert.AreEqual(checkpointPosition, pacmanPosition, "Pacman deber�a reaparecer en la posici�n del checkpoint activo.");
    }
}

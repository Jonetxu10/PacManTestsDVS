using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CheckpointTests_PM
{
    private GameObject pacman;
    private GameObject ghost;
    private CheckPoint newCheckPoint;

    [SetUp]
    public void Setup()
    {
        pacman = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Pacman"), new Vector3(0, 0, 0), Quaternion.identity);

        ghost = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Ghost_Base"), new Vector3(20, 0, 0), Quaternion.identity);
    }

    [TearDown]
    public void TearDown()
    {
        GameObject.Destroy(pacman);
        GameObject.Destroy(ghost);
        CheckPoint.CheckPointsList.Clear();
    }

    [UnityTest]
    public IEnumerator CheckPointIsCreated()
    {
        GameObject cp = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Checkpoint"), new Vector3(50, 0, 0), Quaternion.identity);
        cp.GetComponent<CheckPoint>();
        yield return new WaitForSeconds(1f);

        Assert.AreEqual(1, CheckPoint.CheckPointsList.Count, "Solo 1 CheckPoint creado"); // Porque este es el primero que se crea
        Assert.False(cp.GetComponent<CheckPoint>().Activated, "Checkpoint no activo"); // Devuelve true cuando lo de dentro es falso - aquí comprobamos que el que se ha creado aún no está activo
    }
    /*
    [UnityTest]
    public async IEnumerator CheckPointIsActivated()
    {
        yield return CheckPointIsCreated(); // Primero tiene que haberse creado - Si el primero no se inicia este test no se ejecutará
        pacman.transform.position = newCheckPoint.position; // El jugador tiene que pasar
        yield return WaitForSeconds(0.1f);// Espero a que el jugador pase
        Assert.True(newCheckPoint.Activated, "Checkpoint activo"); // Devuelve true cuando lo de dentro es true - aquí comprobamos que se ha activado            
    }*/
    [UnityTest]
    public IEnumerator PlayerAppearsInCheckPointActivated()
    {
        yield return null;
    }
}


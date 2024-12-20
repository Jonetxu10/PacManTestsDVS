using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CheckpointTests_PM
{
    /*private GameObject player;
    private GameObject enemy;
    private CheckPoint newCheckPoint;

    [SetUp]
    public void Setup()
    {
        player = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"), new Vector3(0, 0, 0), Quaternion.identity);
        player.GetComponent<RigidBody>().useGravity = false;

        enemy = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Enemy"), new Vector3(200, 0, 0), Quaternion.identity);
        enemy.GetComponent<RigidBody>().useGravity = false;
    }

    [TearDown]
    public void TearDown()
    {
        GameObject.Destroy(player);
        GameObject.Destroy(enemy);
        GameObject.Destroy(newCheckPoint.gameObject);
        CheckPoint.CheckPointLists.Clear();
    }

    [UnityTest]
    public IEnumerator CheckPointIsCreated()
    {
        GameObject cp = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/CheckPoint"), new Vector3(100, 0, 0), Quaternion.identity);
        //cp.GetComponent<RigidBody>().useGravity = false;
        cp.GetComponent<CheckPoint>();
        yield return new WaitForSeconds(1f);

        Assert.AreEqual(1, CheckPoint.CheckPointList.Count, "Solo 1 CheckPoint creado"); // Porque este es el primero que se crea
        Assert.False(cp.Activated, "Checkpoint no activo"); // Devuelve true cuando lo de dentro es falso - aquí comprobamos que el que se ha creado aún no está activo
    }

    [UnityTest]
    public async IEnumerator CheckPointIsActivated()
    {
        yield return CheckPointIsCreated(); // Primero tiene que haberse creado - Si el primero no se inicia este test no se ejecutará
        player.transform.position = newCheckPoint.position; // El jugador tiene que pasar
        yield return WaitForSeconds(0.1f);// Espero a que el jugador pase
        Assert.True(newCheckPoint.Activated, "Checkpoint activo"); // Devuelve true cuando lo de dentro es true - aquí comprobamos que se ha activado            
    }
    [UnityTest]
    public IEnumerator PlayerAppearsInCheckPointActivated()
    {
        yield return null;
    }*/
}

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/* NOMBRE CLASE:  PassageTest_PM
 * AUTOR: Diego Hidalgo Delgado
 * FECHA: 19/12/2024
 * VERSIÓN: 1.0 test y todo lo necesario para que funcione
 * DESCRIPCIÓN: Clase que testea todo lo que tiene que ver con Passage en PlayMode
 *                 - Comprueba que al entrar en el trigger el objeto que ha entrado cambie de posición al otro extremo
 *                 - Que se mantenga activo
 *                 - Correcto funcionamiento al pasar más de un GameObject a la vez
 */
public class PassageTest_PM
{
    private GameObject passageGameObject;
    private Passage passage;
    private GameObject otherGameObject;
    private GameObject anotherGameObject;
    private Transform connection;

    [SetUp]
    public void Setup()
    {
        passageGameObject = new GameObject();
        passageGameObject.AddComponent<BoxCollider2D>();
        passage = passageGameObject.AddComponent<Passage>();

        var connectionGameObject = new GameObject();
        connection = connectionGameObject.transform;
        connection.position = new Vector3(5, 5, 0);
        passage.connection = connection;

        otherGameObject = new GameObject();
        otherGameObject.AddComponent<BoxCollider2D>();
        anotherGameObject = new GameObject();
        anotherGameObject.AddComponent<BoxCollider2D>();
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(passageGameObject);
        Object.Destroy(otherGameObject);
        Object.Destroy(anotherGameObject);
        Object.Destroy(connection.gameObject);
    }

    [UnityTest]
    public IEnumerator OnTriggerEnter2DMovesObjectToConnectionPosition()
    {
        otherGameObject.transform.position = new Vector3(0, 0, 1);
        yield return new WaitForFixedUpdate(); 

        SimulateTriggerEnter2D(passage, otherGameObject);
        yield return new WaitForFixedUpdate();
        Vector3 expectedPosition = connection.position;
        expectedPosition.z = otherGameObject.transform.position.z;

        Assert.AreEqual(expectedPosition, otherGameObject.transform.position, "El objeto no se ha movido al otro extremo.");
    }

    [UnityTest]
    public IEnumerator OnTriggerEnter2DChangesStateAfterCollision()
    {
        otherGameObject.transform.position = new Vector3(0, 0, 1);
        yield return new WaitForFixedUpdate(); 

        SimulateTriggerEnter2D(passage, otherGameObject);
        yield return new WaitForFixedUpdate(); 
        
        Assert.IsTrue(otherGameObject.activeSelf, "El objeto no está activado");
    }

    [UnityTest]
    public IEnumerator OnTriggerEnter2DHandlesMultipleCollisions()
    {
        otherGameObject.transform.position = new Vector3(0, 0, 1);
        anotherGameObject.transform.position = new Vector3(1, 0, 1);
        yield return new WaitForFixedUpdate(); 

        SimulateTriggerEnter2D(passage, otherGameObject);
        yield return new WaitForFixedUpdate(); 
        Vector3 expectedPosition = connection.position;
        expectedPosition.z = otherGameObject.transform.position.z;
        Assert.AreEqual(expectedPosition, otherGameObject.transform.position, "El primer objeto no se ha movido al otro extremo");

        SimulateTriggerEnter2D(passage, anotherGameObject);
        yield return new WaitForFixedUpdate(); 
        expectedPosition.z = anotherGameObject.transform.position.z;
        Assert.AreEqual(expectedPosition, anotherGameObject.transform.position, "El segundo objeto no se ha movido al otro extremo");
    }

    /* NOMBRE MÉTODO: SimulateTriggerEnter2D
     * AUTOR: Diego Hidalgo Delgado
     * FECHA: 19/12/2024
     * DESCRIPCIÓN: Simula la colisión entre los objetos mediante TriggerEnter
     * @param: Passage passage
     *         GameObject other
     * @return: -
     */

    private void SimulateTriggerEnter2D(Passage passage, GameObject other)
    {
        var method = typeof(Passage).GetMethod("OnTriggerEnter2D", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        method.Invoke(passage, new object[] { other.GetComponent<Collider2D>() });
    }
}

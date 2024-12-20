using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/* NOMBRE CLASE: PassageTest_EM
 * AUTOR: Diego Hidalgo Delgado
 * FECHA: 18/12/2024
 * VERSIÓN: 1.0 test y todo lo necesario para que funcione
 * DESCRIPCIÓN: Comprueba que el Passage funcione correctamente, que esté asignado y mueva al objeto de un lado a otro
 */
public class PassageTest_EM
{
    private GameObject passageGameObject;
    private Passage passage;
    private GameObject otherGameObject;
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
    }

    [Test]
    public void ConnectionIsAssigned()
    {
        Assert.IsNotNull(passage.connection, "La conexión no se ha asignado debidamente.");
    }

    [Test]
    public void OnTriggerEnter2DMovesObjectToConnectionPosition()
    {
        otherGameObject.transform.position = new Vector3(0, 0, 1);

        SimulateTriggerEnter2D(passage, otherGameObject);

        Vector3 expectedPosition = connection.position;
        expectedPosition.z = otherGameObject.transform.position.z;

        Assert.AreEqual(expectedPosition, otherGameObject.transform.position, "El Passage no mueve al objeto a la posición que debe");
    }

    /* NOMBRE MÉTODO: SimulateTriggerEnter2D
     * AUTOR: Diego Hidalgo Delgado
     * FECHA: 18/12/2024
     * DESCRIPCIÓN: Simula la entrada en el trigger para comprobar que funciona
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

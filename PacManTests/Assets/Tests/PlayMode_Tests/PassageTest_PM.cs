using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

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
        // Crear el objeto Passage y agregar el colisionador antes del componente Passage
        passageGameObject = new GameObject();
        passageGameObject.AddComponent<BoxCollider2D>(); // Agregar el colisionador primero
        passage = passageGameObject.AddComponent<Passage>();

        // Crear el objeto de conexión y asignarlo
        var connectionGameObject = new GameObject();
        connection = connectionGameObject.transform;
        connection.position = new Vector3(5, 5, 0);
        passage.connection = connection;

        // Crear el otro objeto que colisionará
        otherGameObject = new GameObject();
        otherGameObject.AddComponent<BoxCollider2D>();
        anotherGameObject = new GameObject();
        anotherGameObject.AddComponent<BoxCollider2D>();
    }

    [UnityTest]
    public IEnumerator OnTriggerEnter2D_MovesObjectToConnectionPosition_InPlayMode()
    {
        // Configurar la posición inicial del otro objeto
        otherGameObject.transform.position = new Vector3(0, 0, 1);

        // Habilitar los colisionadores y simular la colisión
        yield return new WaitForFixedUpdate(); // Esperar a la actualización de física

        // Simular la colisión usando un script auxiliar
        SimulateTriggerEnter2D(passage, otherGameObject);

        yield return new WaitForFixedUpdate(); // Esperar a la actualización de física

        // Verificar que la posición del otro objeto sea igual a la posición de la conexión
        Vector3 expectedPosition = connection.position;
        expectedPosition.z = otherGameObject.transform.position.z; // Asegurarse de que la posición Z se mantiene

        Assert.AreEqual(expectedPosition, otherGameObject.transform.position);
    }

    [UnityTest]
    public IEnumerator OnTriggerEnter2D_ChangesStateAfterCollision()
    {
        // Configurar la posición inicial del otro objeto
        otherGameObject.transform.position = new Vector3(0, 0, 1);

        // Habilitar los colisionadores y simular la colisión
        yield return new WaitForFixedUpdate(); // Esperar a la actualización de física

        SimulateTriggerEnter2D(passage, otherGameObject);

        yield return new WaitForFixedUpdate(); // Esperar a la actualización de física

        // Verificar que la posición del otro objeto sea igual a la posición de la conexión
        Vector3 expectedPosition = connection.position;
        expectedPosition.z = otherGameObject.transform.position.z;

        Assert.AreEqual(expectedPosition, otherGameObject.transform.position);
        Assert.IsTrue(otherGameObject.activeSelf); // Comprobar que el objeto sigue activo
    }

    [UnityTest]
    public IEnumerator OnTriggerEnter2D_HandlesMultipleCollisions()
    {
        // Configurar la posición inicial del primer objeto
        otherGameObject.transform.position = new Vector3(0, 0, 1);
        // Configurar la posición inicial del segundo objeto
        anotherGameObject.transform.position = new Vector3(1, 0, 1);

        yield return new WaitForFixedUpdate(); // Esperar a la actualización de física

        // Simular la primera colisión
        SimulateTriggerEnter2D(passage, otherGameObject);

        yield return new WaitForFixedUpdate(); // Esperar a la actualización de física

        // Verificar que la posición del primer objeto sea igual a la posición de la conexión
        Vector3 expectedPosition = connection.position;
        expectedPosition.z = otherGameObject.transform.position.z;
        Assert.AreEqual(expectedPosition, otherGameObject.transform.position);

        // Simular la segunda colisión
        SimulateTriggerEnter2D(passage, anotherGameObject);

        yield return new WaitForFixedUpdate(); // Esperar a la actualización de física

        // Verificar que la posición del segundo objeto sea igual a la posición de la conexión
        expectedPosition.z = anotherGameObject.transform.position.z;
        Assert.AreEqual(expectedPosition, anotherGameObject.transform.position);
    }

    private void SimulateTriggerEnter2D(Passage passage, GameObject other)
    {
        // Usar reflexión para llamar al método OnTriggerEnter2D
        var method = typeof(Passage).GetMethod("OnTriggerEnter2D", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        method.Invoke(passage, new object[] { other.GetComponent<Collider2D>() });
    }

    [TearDown]
    public void Teardown()
    {
        // Limpiar los objetos creados
        // Limpiar los objetos creados
        Object.Destroy(passageGameObject);
        Object.Destroy(otherGameObject);
        Object.Destroy(anotherGameObject);
        Object.Destroy(connection.gameObject);
    }




}

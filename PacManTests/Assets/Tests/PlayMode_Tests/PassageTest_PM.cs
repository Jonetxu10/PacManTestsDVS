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

        // Crear el objeto de conexi�n y asignarlo
        var connectionGameObject = new GameObject();
        connection = connectionGameObject.transform;
        connection.position = new Vector3(5, 5, 0);
        passage.connection = connection;

        // Crear el otro objeto que colisionar�
        otherGameObject = new GameObject();
        otherGameObject.AddComponent<BoxCollider2D>();
        anotherGameObject = new GameObject();
        anotherGameObject.AddComponent<BoxCollider2D>();
    }

    [UnityTest]
    public IEnumerator OnTriggerEnter2D_MovesObjectToConnectionPosition_InPlayMode()
    {
        // Configurar la posici�n inicial del otro objeto
        otherGameObject.transform.position = new Vector3(0, 0, 1);

        // Habilitar los colisionadores y simular la colisi�n
        yield return new WaitForFixedUpdate(); // Esperar a la actualizaci�n de f�sica

        // Simular la colisi�n usando un script auxiliar
        SimulateTriggerEnter2D(passage, otherGameObject);

        yield return new WaitForFixedUpdate(); // Esperar a la actualizaci�n de f�sica

        // Verificar que la posici�n del otro objeto sea igual a la posici�n de la conexi�n
        Vector3 expectedPosition = connection.position;
        expectedPosition.z = otherGameObject.transform.position.z; // Asegurarse de que la posici�n Z se mantiene

        Assert.AreEqual(expectedPosition, otherGameObject.transform.position);
    }

    [UnityTest]
    public IEnumerator OnTriggerEnter2D_ChangesStateAfterCollision()
    {
        // Configurar la posici�n inicial del otro objeto
        otherGameObject.transform.position = new Vector3(0, 0, 1);

        // Habilitar los colisionadores y simular la colisi�n
        yield return new WaitForFixedUpdate(); // Esperar a la actualizaci�n de f�sica

        SimulateTriggerEnter2D(passage, otherGameObject);

        yield return new WaitForFixedUpdate(); // Esperar a la actualizaci�n de f�sica

        // Verificar que la posici�n del otro objeto sea igual a la posici�n de la conexi�n
        Vector3 expectedPosition = connection.position;
        expectedPosition.z = otherGameObject.transform.position.z;

        Assert.AreEqual(expectedPosition, otherGameObject.transform.position);
        Assert.IsTrue(otherGameObject.activeSelf); // Comprobar que el objeto sigue activo
    }

    [UnityTest]
    public IEnumerator OnTriggerEnter2D_HandlesMultipleCollisions()
    {
        // Configurar la posici�n inicial del primer objeto
        otherGameObject.transform.position = new Vector3(0, 0, 1);
        // Configurar la posici�n inicial del segundo objeto
        anotherGameObject.transform.position = new Vector3(1, 0, 1);

        yield return new WaitForFixedUpdate(); // Esperar a la actualizaci�n de f�sica

        // Simular la primera colisi�n
        SimulateTriggerEnter2D(passage, otherGameObject);

        yield return new WaitForFixedUpdate(); // Esperar a la actualizaci�n de f�sica

        // Verificar que la posici�n del primer objeto sea igual a la posici�n de la conexi�n
        Vector3 expectedPosition = connection.position;
        expectedPosition.z = otherGameObject.transform.position.z;
        Assert.AreEqual(expectedPosition, otherGameObject.transform.position);

        // Simular la segunda colisi�n
        SimulateTriggerEnter2D(passage, anotherGameObject);

        yield return new WaitForFixedUpdate(); // Esperar a la actualizaci�n de f�sica

        // Verificar que la posici�n del segundo objeto sea igual a la posici�n de la conexi�n
        expectedPosition.z = anotherGameObject.transform.position.z;
        Assert.AreEqual(expectedPosition, anotherGameObject.transform.position);
    }

    private void SimulateTriggerEnter2D(Passage passage, GameObject other)
    {
        // Usar reflexi�n para llamar al m�todo OnTriggerEnter2D
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

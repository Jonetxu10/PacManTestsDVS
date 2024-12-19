using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PassageTest_EM
{
    private GameObject passageGameObject;
    private Passage passage;
    private GameObject otherGameObject;
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
    }

    [Test]
    public void Connection_IsAssigned()
    {
        Assert.IsNotNull(passage.connection);
    }

    [Test]
    public void OnTriggerEnter2D_MovesObjectToConnectionPosition()
    {
        // Configurar la posición inicial del otro objeto
        otherGameObject.transform.position = new Vector3(0, 0, 1);

        // Simular la colisión usando un script auxiliar
        SimulateTriggerEnter2D(passage, otherGameObject);

        // Verificar que la posición del otro objeto sea igual a la posición de la conexión
        Vector3 expectedPosition = connection.position;
        expectedPosition.z = otherGameObject.transform.position.z; // Asegurarse de que la posición Z se mantiene

        Assert.AreEqual(expectedPosition, otherGameObject.transform.position);
    }

    private void SimulateTriggerEnter2D(Passage passage, GameObject other)
    {
        // Usar reflexión para llamar al método OnTriggerEnter2D
        var method = typeof(Passage).GetMethod("OnTriggerEnter2D", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        method.Invoke(passage, new object[] { other.GetComponent<Collider2D>() });
    }




  



}

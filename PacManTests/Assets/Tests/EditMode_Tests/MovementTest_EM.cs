using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.Tilemaps;

/* NOMBRE CLASE: MovementTest_EM
 * AUTOR: Jone Sainz Egea
 * FECHA: 17/12/2024
 * VERSIÓN: 1.0 estado inicial y reseteo
 *              1.1 caso de que haya obstáculo
 *              1.2 caso de que no haya obstácuo
 * DESCRIPCIÓN: Clase que se encarga de testear todo lo relacionado con Movement en Edit Mode
 *                  - Comprueba que el estado inicial es correcto
 *                  - El reseteo de movimiento funciona correctamente
 *                  - isOccupied devuelve true cuando hay un obstáculo en el camino
 *                  - La dirección se actualiza correctamente cuando no hay obstáculos
 *                  - La siguiente dirección nse done en cola cuando hay un obstáculo
 *                  - La dirección se actualiza cuando está forzado
 */
public class MovementTest_EM
{
    private GameObject movementGameObject;
    private Movement movement;

    [SetUp]
    public void SetUp()
    {
        movementGameObject = new GameObject();
        movementGameObject.AddComponent<Rigidbody2D>();
        movementGameObject.GetComponent<Rigidbody2D>().isKinematic = false;
        movement = movementGameObject.AddComponent<Movement>();
        movement.initialDirection = Vector2.right;

        movement.Awake();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(movementGameObject);
    }

    [Test]
    public void MovementInitialStateIsCorrect()
    {
        Assert.AreEqual(Vector2.zero, movement.direction, "La dirección inicial no es cero.");
        Assert.AreEqual(Vector2.zero, movement.nextDirection, "La siguiente dirección no es cero.");
    }

    [Test]
    public void MovementResetStateResetsPositionAndDirection()
    {
        movement.ResetState();

        Assert.AreEqual(Vector3.zero, movement.transform.position, "La posición no se ha reseteado.");
        Assert.AreEqual(Vector2.right, movement.direction, "La dirección no se ha reseteado");
    }

    [Test]
    public void MovementOccupiedReturnsTrueWhenObstacleInDirection()
    {
        movement.obstacleLayer = LayerMask.GetMask("Obstacle");
        GameObject obstacle = new GameObject();
        obstacle.AddComponent<BoxCollider2D>();
        obstacle.layer = LayerMask.NameToLayer("Obstacle");
        obstacle.transform.position = movementGameObject.transform.position + Vector3.up;

        bool isOccupied = movement.Occupied(Vector2.up);

        Assert.IsTrue(isOccupied, "isOccupied no devuelve true cuando hay un obstáculo en el camino.");
    }

    [Test]
    public void SetDirectionUpdatesDirectionWhenNotOccupied()
    {
        movement.SetDirection(Vector2.down);

        Assert.AreEqual(Vector2.down, movement.direction, "La dirección no se ha actualizado sin que haya obstáculos.");
        Assert.AreEqual(Vector2.zero, movement.nextDirection, "La siguiente dirección no se ha reseteado a cero.");
    }

    [Test]
    public void SetDirectionQueuesNextDirectionWhenOccupied()
    {
        movement.obstacleLayer = LayerMask.GetMask("Obstacle");

        GameObject obstacle = new GameObject("Obstacle");
        obstacle.AddComponent<BoxCollider2D>();
        obstacle.layer = LayerMask.NameToLayer("Obstacle");
        obstacle.transform.position = movementGameObject.transform.position + Vector3.left;

        movement.enabled = false;

        movement.SetDirection(Vector2.left);

        Assert.AreEqual(Vector2.left, movement.nextDirection, "La siguiente dirección no se ha puesto en cola al haber un obstáculo.");
    }

    [Test]
    public void SetDirectionForcesDirectionWhenForced()
    {
        movement.obstacleLayer = LayerMask.GetMask("Obstacle");
        GameObject obstacle = new GameObject();
        obstacle.AddComponent<BoxCollider2D>();
        obstacle.layer = LayerMask.NameToLayer("Obstacle");
        obstacle.transform.position = movementGameObject.transform.position + Vector3.left;

        movement.SetDirection(Vector2.left, forced: true); 

        Assert.AreEqual(Vector2.left, movement.direction, "La dirección no se actualiza cuando está forzado.");
        Assert.AreEqual(Vector2.zero, movement.nextDirection, "La siguiente dirección no se resestea al estar forzado.");
    }
}

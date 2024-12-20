using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.Tilemaps;

/* NOMBRE CLASE: MovementTest_EM
 * AUTOR: Jone Sainz Egea
 * FECHA: 17/12/2024
 * VERSI�N: 1.0 estado inicial y reseteo
 *              1.1 caso de que haya obst�culo
 *              1.2 caso de que no haya obst�cuo
 * DESCRIPCI�N: Clase que se encarga de testear todo lo relacionado con Movement en Edit Mode
 *                  - Comprueba que el estado inicial es correcto
 *                  - El reseteo de movimiento funciona correctamente
 *                  - isOccupied devuelve true cuando hay un obst�culo en el camino
 *                  - La direcci�n se actualiza correctamente cuando no hay obst�culos
 *                  - La siguiente direcci�n nse done en cola cuando hay un obst�culo
 *                  - La direcci�n se actualiza cuando est� forzado
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
        Assert.AreEqual(Vector2.zero, movement.direction, "La direcci�n inicial no es cero.");
        Assert.AreEqual(Vector2.zero, movement.nextDirection, "La siguiente direcci�n no es cero.");
    }

    [Test]
    public void MovementResetStateResetsPositionAndDirection()
    {
        movement.ResetState();

        Assert.AreEqual(Vector3.zero, movement.transform.position, "La posici�n no se ha reseteado.");
        Assert.AreEqual(Vector2.right, movement.direction, "La direcci�n no se ha reseteado");
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

        Assert.IsTrue(isOccupied, "isOccupied no devuelve true cuando hay un obst�culo en el camino.");
    }

    [Test]
    public void SetDirectionUpdatesDirectionWhenNotOccupied()
    {
        movement.SetDirection(Vector2.down);

        Assert.AreEqual(Vector2.down, movement.direction, "La direcci�n no se ha actualizado sin que haya obst�culos.");
        Assert.AreEqual(Vector2.zero, movement.nextDirection, "La siguiente direcci�n no se ha reseteado a cero.");
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

        Assert.AreEqual(Vector2.left, movement.nextDirection, "La siguiente direcci�n no se ha puesto en cola al haber un obst�culo.");
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

        Assert.AreEqual(Vector2.left, movement.direction, "La direcci�n no se actualiza cuando est� forzado.");
        Assert.AreEqual(Vector2.zero, movement.nextDirection, "La siguiente direcci�n no se resestea al estar forzado.");
    }
}

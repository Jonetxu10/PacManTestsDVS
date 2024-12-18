using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.Tilemaps;

public class MovementTest_EM
{
    private GameObject movementObject;
    private Movement movement;
    Vector3 expectedStartingPosition = new Vector3(0f, -9.5f, -5f);
    Vector2 expectedInitialDirection = Vector2.right;

    [SetUp]
    public void SetUp()
    {
        movementObject = new GameObject();
        movementObject.AddComponent<Rigidbody2D>();
        movementObject.GetComponent<Rigidbody2D>().isKinematic = false;
        movement = movementObject.AddComponent<Movement>();
        movement.initialDirection = expectedInitialDirection;

        movement.Awake();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(movementObject);
    }

    [Test]
    public void Movement_InitialState_IsCorrect()
    {
        Assert.AreEqual(Vector2.right, movement.direction, "Initial direction should be right.");
        Assert.AreEqual(Vector2.zero, movement.nextDirection, "Next direction should be zero.");
    }

    [Test]
    public void Movement_ResetState_ResetsPositionAndDirection()
    {
        movementObject.transform.position = expectedStartingPosition;
        movement.initialDirection = expectedInitialDirection;

        movement.ResetState();

        Assert.AreEqual(expectedStartingPosition, movement.transform.position, "Position should be reset.");
        Assert.AreEqual(expectedInitialDirection, movement.direction, "Direction should be reset.");
    }

    [Test]
    public void Movement_Occupied_ReturnsTrue_WhenObstacleInDirection()
    {
        movement.obstacleLayer = LayerMask.GetMask("Obstacle");

        GameObject obstacle = new GameObject();
        obstacle.AddComponent<BoxCollider2D>();
        obstacle.layer = LayerMask.NameToLayer("Obstacle");
        obstacle.transform.position = movementObject.transform.position + Vector3.up;

        bool isOccupied = movement.Occupied(Vector2.up);

        Assert.IsTrue(isOccupied, "Occupied should return true when an obstacle exists.");
    }

    [Test]
    public void SetDirection_UpdatesDirection_WhenNotOccupied()
    {
        movement.SetDirection(Vector2.down);

        Assert.AreEqual(Vector2.down, movement.direction, "The direction should update when there are no obstacles.");
        Assert.AreEqual(Vector2.zero, movement.nextDirection, "Next direction should be reset to zero.");
    }

    [Test]
    public void SetDirection_QueuesNextDirection_WhenOccupied()
    {
        movement.obstacleLayer = LayerMask.GetMask("Obstacle");

        GameObject obstacle = new GameObject("Obstacle");
        obstacle.AddComponent<BoxCollider2D>();
        obstacle.layer = LayerMask.NameToLayer("Obstacle");
        obstacle.transform.position = movementObject.transform.position + Vector3.left;

        movement.enabled = false;

        movement.SetDirection(Vector2.left);

        Assert.AreEqual(Vector2.left, movement.nextDirection, "Next direction should be queued when there is an obstacle.");
    }

    [Test]
    public void SetDirection_ForcesDirection_WhenForced()
    {
        movement.obstacleLayer = LayerMask.GetMask("Obstacle");
        GameObject obstacle = new GameObject();
        obstacle.AddComponent<BoxCollider2D>();
        obstacle.layer = LayerMask.NameToLayer("Obstacle");
        obstacle.transform.position = movementObject.transform.position + Vector3.left;

        movement.SetDirection(Vector2.left, forced: true); 

        Assert.AreEqual(Vector2.left, movement.direction, "Direction should update when forced.");
        Assert.AreEqual(Vector2.zero, movement.nextDirection, "Next direction should be reset when forced.");
    }
}

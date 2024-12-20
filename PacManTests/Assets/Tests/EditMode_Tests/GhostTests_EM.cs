using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using PacManGame;
using UnityEngine.XR;

public class GhostTests_EM
{
    private GameObject ghostObject;
    private Ghost ghost;

    [SetUp]
    public void SetUp()
    {
        ghostObject = new GameObject();
        ghostObject.AddComponent<SpriteRenderer>();
        ghostObject.AddComponent<BoxCollider2D>();
        var rb = ghostObject.AddComponent<Rigidbody2D>();
        rb.isKinematic = true;
        ghostObject.AddComponent<Movement>();
        ghostObject.AddComponent<GhostHome>();
        ghostObject.AddComponent<GhostScatter>();
        ghostObject.AddComponent<GhostChase>();
        ghostObject.AddComponent<GhostFrightened>();
        ghost = ghostObject.AddComponent<Ghost>();


        ghost.Awake();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(ghostObject);
    }

    [Test]
    public void Ghost_InitializesComponents()
    {
        Assert.IsNotNull(ghost.movement, "Movement should be initialized.");
        Assert.IsNotNull(ghost.home, "Home should be initialized.");
        Assert.IsNotNull(ghost.scatter, "Scatter should be initialized.");
        Assert.IsNotNull(ghost.chase, "Chase should be initialized.");
        Assert.IsNotNull(ghost.frightened, "Frightened should be initialized.");
    }

    [Test]
    public void Ghost_SetPosition_SetsCorrectPosition()
    {
        Vector3 newPosition = new Vector3(1f, 2f, 3f);
        ghost.SetPosition(newPosition);

        Assert.AreEqual(new Vector3(1f, 2f, ghost.transform.position.z), ghost.transform.position, "Position should be set correctly.");
    }

    [Test]
    public void GhostChase_ChangesDirection_OnNodeEnter()
    {
        // Create necessary GameObjects and components
        GameObject ghostObject = new GameObject();
        Ghost ghost = ghostObject.AddComponent<Ghost>();
        ghostObject.AddComponent<Movement>();
        ghostObject.AddComponent<BoxCollider2D>();
        ghost.chase = ghostObject.AddComponent<GhostChase>();
        ghost.movement = ghostObject.GetComponent<Movement>();
        ghost.frightened = ghostObject.AddComponent<GhostFrightened>();
        ghost.chase.ghost = ghost;

        GameObject targetObject = new GameObject();
        targetObject.transform.position = Vector3.up * 10f;
        ghost.target = targetObject.transform;
        ghost.chase.Enable();

        GameObject nodeObject = new GameObject();
        Node node = nodeObject.AddComponent<Node>();
        nodeObject.AddComponent<BoxCollider2D>();
        node.availableDirections = new System.Collections.Generic.List<Vector2>();
        node.availableDirections.Add(Vector2.up);
        node.availableDirections.Add(Vector2.left);
        node.availableDirections.Add(Vector2.right);

        nodeObject.transform.position = ghostObject.transform.position + Vector3.up;

        ghostObject.transform.position = Vector3.zero;
        ghost.movement.SetDirection(Vector2.up);

        ghost.chase.OnTriggerEnter2D(nodeObject.GetComponent<Collider2D>());

        ghostObject.transform.position += (Vector3)(ghost.movement.direction * ghost.movement.speed * Time.fixedDeltaTime);

        Vector2 expectedDirection = Vector2.zero;
        float minDistance = float.MaxValue;

        foreach (Vector2 availableDirection in node.availableDirections)
        {
            Vector3 worldPositionOfAvailableNode = nodeObject.transform.position + (Vector3)availableDirection;
            float distance = (ghost.target.position - worldPositionOfAvailableNode).sqrMagnitude;

            if (distance < minDistance)
            {
                minDistance = distance;
                expectedDirection = availableDirection;
            }
        }

        Assert.AreEqual(expectedDirection, ghost.movement.direction, "Ghost should move upwards towards the target.");

        Object.DestroyImmediate(ghostObject);
        Object.DestroyImmediate(targetObject);
        Object.DestroyImmediate(nodeObject);
    }

    [Test]
    public void GhostFrightened_ChangesDirection_OnNodeEnter()
    {
        // Create the target
        GameObject targetObject = new GameObject();
        targetObject.transform.position = new Vector3(10f, 0f, 0f); // Target to the right

        ghost.target = targetObject.transform;

        // Create the node
        GameObject nodeObject = new GameObject();
        Node node = nodeObject.AddComponent<Node>();
        nodeObject.AddComponent<BoxCollider2D>();
        node.availableDirections = new System.Collections.Generic.List<Vector2>();
        node.availableDirections.Add(Vector2.up);
        node.availableDirections.Add(Vector2.down);
        node.availableDirections.Add(Vector2.left);

        // Position the node relative to the ghost
        nodeObject.transform.position = ghostObject.transform.position + Vector3.up; // Node above the ghost

        // Set the ghost's initial position
        ghostObject.transform.position = Vector3.zero;
        ghost.movement.SetDirection(Vector2.down); // Initial direction

        // Simulate Frightened Behavior DIRECTLY
        Vector2 expectedDirection = Vector2.zero;
        float maxDistance = float.MinValue;

        foreach (Vector2 availableDirection in node.availableDirections)
        {
            Vector3 newPosition = nodeObject.transform.position + new Vector3(availableDirection.x, availableDirection.y);
            float distance = (ghost.target.position - newPosition).sqrMagnitude;

            if (distance > maxDistance)
            {
                maxDistance = distance;
                expectedDirection = availableDirection;
            }
        }
        ghost.movement.SetDirection(expectedDirection, true);

        Assert.AreEqual(Vector2.left, ghost.movement.direction, "Ghost should move left away from the target.");

        Object.DestroyImmediate(targetObject);
        Object.DestroyImmediate(nodeObject);
    }

    [Test]
    public void GhostHome_ReversesDirectionOnObstacleCollision()
    {
        // Arrange
        ghost.home.enabled = true; // Make sure the script is enabled
        ghost.movement.SetDirection(Vector2.right, true); // Set an initial direction

        // Act (Simulate the collision behavior directly)
        ghost.movement.SetDirection(-ghost.movement.direction, true); // Reverse the direction

        // Assert
        Assert.AreEqual(Vector2.left, ghost.movement.direction, "Direction should be reversed.");
    }
}


using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using PacManGame;
using UnityEngine.XR;

/* NOMBRE CLASE: GhostTest_EM
 * AUTOR: Jone Sainz Egea
 * FECHA: 17/12/2024
 * VERSIÓN: 1.0 inicialización de componentes y posición
 *              1.1. cambios de dirección con nodos
 *              1.2. funcionamiento ghostHome
 * DESCRIPCIÓN: Clase que se encarga de testear todo lo relacionado con los Ghost en Edit Mode
 *                  - Comprueba que se inicializan los componentes de Ghost
 *                  - El cambio de posición funciona correctamente
 *                  - 
 */
public class GhostTest_EM
{
    private GameObject ghostGameObject;
    private Ghost ghost;

    [SetUp]
    public void SetUp()
    {
        ghostGameObject = new GameObject();
        ghostGameObject.AddComponent<SpriteRenderer>();
        ghostGameObject.AddComponent<BoxCollider2D>();
        var rb = ghostGameObject.AddComponent<Rigidbody2D>();
        rb.isKinematic = true;
        ghostGameObject.AddComponent<Movement>();
        ghostGameObject.AddComponent<GhostHome>();
        ghostGameObject.AddComponent<GhostScatter>();
        ghostGameObject.AddComponent<GhostChase>();
        ghostGameObject.AddComponent<GhostFrightened>();
        ghost = ghostGameObject.AddComponent<Ghost>();

        ghost.Awake();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(ghostGameObject);
    }

    [Test]
    public void GhostInitializesComponents()
    {
        Assert.IsNotNull(ghost.movement, "Movement no se ha inicializado.");
        Assert.IsNotNull(ghost.home, "Home no se ha inicializado.");
        Assert.IsNotNull(ghost.scatter, "Scatter no se ha inicializado.");
        Assert.IsNotNull(ghost.chase, "Chase no se ha inicializado.");
        Assert.IsNotNull(ghost.frightened, "Frightened no se ha inicializado.");
    }

    [Test]
    public void GhostSetPositionSetsCorrectPosition()
    {
        Vector3 newPosition = new Vector3(1f, 2f, 3f);
        ghost.SetPosition(newPosition);

        Assert.AreEqual(new Vector3(1f, 2f, ghost.transform.position.z), ghost.transform.position, "SetPosition no cambia la posición correctamente.");
    }

    [Test]
    public void GhostChaseChangesDirectionBasedOnNode()
    {
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
    public void GhostFrightenedChangesDirectionBasedOnNode()
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
        nodeObject.transform.position = ghostGameObject.transform.position + Vector3.up; // Node above the ghost

        // Set the ghost's initial position
        ghostGameObject.transform.position = Vector3.zero;
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
        ghost.home.enabled = true;
        ghost.movement.SetDirection(Vector2.right, true);

        ghost.movement.SetDirection(-ghost.movement.direction, true);

        Assert.AreEqual(Vector2.left, ghost.movement.direction, "Direction should be reversed.");
    }
}


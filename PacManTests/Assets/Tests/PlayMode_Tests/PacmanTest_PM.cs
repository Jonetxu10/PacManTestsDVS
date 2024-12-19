using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

public class PacmanTest_PM
{
    private GameObject pacmanGameObject;
    private Pacman pacman;
    private Movement movement;

    [SetUp]
    public void SetUp()
    {
        // Configurar los GameObjects necesarios
        pacmanGameObject = new GameObject("Pacman");
        pacmanGameObject.AddComponent<Rigidbody2D>();
        pacmanGameObject.AddComponent<BoxCollider2D>();
        pacmanGameObject.AddComponent<SpriteRenderer>();

        // Añadir los scripts principales
        pacman = pacmanGameObject.AddComponent<Pacman>();
        movement = pacmanGameObject.AddComponent<Movement>();

        // Configurar valores iniciales
        pacman.deathSequence = pacmanGameObject.AddComponent<AnimatedSprite>();
        movement.initialDirection = Vector2.right;
        movement.speed = 8f;
    }

    [TearDown]
    public void TearDown()
    {
        // Limpiar los GameObjects creados
        Object.Destroy(pacmanGameObject);
    }

    [UnityTest]
    public IEnumerator Pacman_MovesInCorrectDirection_WhenInputIsGiven()
    {
        // Simular entrada de movimiento hacia arriba
        //Input.simulateKeyDown(KeyCode.W);

        yield return null; // Esperar un frame

        Assert.AreEqual(Vector2.up, movement.direction);

        //Input.simulateKeyUp(KeyCode.W); // Limpiar la entrada
    }

    [UnityTest]
    public IEnumerator Pacman_StopsMovement_AfterDeathSequence()
    {
        // Ejecutar la secuencia de muerte
        pacman.DeathSequence();

        yield return null; // Esperar un frame

        Assert.IsFalse(pacman.movement.enabled, "Movement should be disabled after death.");
        Assert.IsFalse(pacman.spriteRenderer.enabled, "SpriteRenderer should be disabled after death.");
    }

    [UnityTest]
    public IEnumerator Movement_DoesNotMove_WhenDirectionIsBlocked()
    {
        // Configurar un obstáculo en la dirección inicial
        GameObject obstacle = new GameObject("Obstacle");
        obstacle.AddComponent<BoxCollider2D>().size = new Vector2(1.5f, 1.5f);
        obstacle.transform.position = pacmanGameObject.transform.position + Vector3.right;

        yield return null; // Esperar un frame para que la física registre el obstáculo

        movement.SetDirection(Vector2.right);
        yield return new WaitForFixedUpdate(); // Esperar a la actualización de FixedUpdate

        // Verificar que Pacman no se mueve
        Vector3 newPosition = pacmanGameObject.transform.position;
        Assert.AreEqual(movement.startingPosition, newPosition);

        Object.Destroy(obstacle);
    }

    [UnityTest]
    public IEnumerator Movement_ChangesDirection_WhenPathIsClear()
    {
        // Cambiar dirección hacia la izquierda sin obstáculos
        movement.SetDirection(Vector2.left);

        yield return new WaitForFixedUpdate(); // Esperar a la actualización de FixedUpdate

        // Verificar que la dirección ha cambiado
        Assert.AreEqual(Vector2.left, movement.direction);
    }

    [UnityTest]
    public IEnumerator Pacman_ResetState_RestoresInitialSettings()
    {
        // Modificar estado de Pacman
        pacman.DeathSequence();

        yield return null; // Esperar un frame

        // Llamar a ResetState
        pacman.ResetState();

        yield return null; // Esperar un frame

        // Verificar que los valores han sido restaurados
        Assert.IsTrue(pacman.enabled, "Pacman should be enabled after reset.");
        Assert.IsTrue(pacman.spriteRenderer.enabled, "SpriteRenderer should be enabled after reset.");
        Assert.IsTrue(pacman.movement.enabled, "Movement should be enabled after reset.");
        Assert.AreEqual(Vector2.right, pacman.movement.direction, "Initial direction should be restored.");
    }
}


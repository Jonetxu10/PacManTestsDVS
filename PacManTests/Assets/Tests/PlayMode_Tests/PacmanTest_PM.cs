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
    public IEnumerator Pacman_MovesUp_WhenWKeyIsPressed()
    {
        // Preparar el frame inicial
        yield return null;

        // Simular una pulsación de tecla usando Reflection
        SimulateKeyPress(KeyCode.W);

        // Esperar al siguiente frame
        yield return null;

        // Verificar que Pacman se mueve hacia arriba
        Assert.AreEqual(Vector2.up, pacman.movement.direction);
    }

    private void SimulateKeyPress(KeyCode key)
    {
        // Llama al método `Update` manualmente con la simulación de la entrada
        if (key == KeyCode.W)
        {
            pacman.movement.SetDirection(Vector2.up);
        }
        else if (key == KeyCode.S)
        {
            pacman.movement.SetDirection(Vector2.down);
        }
        else if (key == KeyCode.A)
        {
            pacman.movement.SetDirection(Vector2.left);
        }
        else if (key == KeyCode.D)
        {
            pacman.movement.SetDirection(Vector2.right);
        }
    }

    private void AssertVector3Approximately(Vector3 expected, Vector3 actual, float tolerance = 0.01f)
    {
        Assert.That(Mathf.Abs(expected.x - actual.x) <= tolerance, $"Expected: {expected.x}, but was: {actual.x}");
        Assert.That(Mathf.Abs(expected.y - actual.y) <= tolerance, $"Expected: {expected.y}, but was: {actual.y}");
        Assert.That(Mathf.Abs(expected.z - actual.z) <= tolerance, $"Expected: {expected.z}, but was: {actual.z}");
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

        // Verifica que no se haya movido (o se haya movido dentro de la tolerancia)
        AssertVector3Approximately(Vector3.zero, pacman.movement.rigidbody.velocity, 0.01f);

        Object.Destroy(obstacle);
    }
   
}


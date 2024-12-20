using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

/* NOMBRE CLASE: PacmanTest_PM
 * AUTOR: Jone Sainz Egea
 * FECHA: 18/12/2024
 * VERSIÓN: 1.0 programa base
 *              1.1. se añade test de camino bloqueado
 * DESCRIPCIÓN: Clase que se encarga de testear todo lo relacionado con el movimiento del personaje jugable
 *                  - Que el personaje se mueva con el input del jugador en la dirección indicada
 */
public class PacmanTest_PM
{
    private GameObject pacmanGameObject;
    private Pacman pacman;
    private Movement movement;

    [SetUp]
    public void SetUp()
    {
        pacmanGameObject = new GameObject("Pacman");
        pacmanGameObject.AddComponent<Rigidbody2D>();
        pacmanGameObject.AddComponent<BoxCollider2D>();
        pacmanGameObject.AddComponent<SpriteRenderer>();
        pacman = pacmanGameObject.AddComponent<Pacman>();
        movement = pacmanGameObject.AddComponent<Movement>();

        pacman.deathSequence = pacmanGameObject.AddComponent<AnimatedSprite>();
        movement.initialDirection = Vector2.right;
        movement.speed = 8f;
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(pacmanGameObject);
    }

    [UnityTest]
    public IEnumerator PacmanMovesUpWhenWKeyIsPressed()
    {
        yield return null;
        SimulateKeyPress(KeyCode.W);
        yield return null;

        Assert.AreEqual(Vector2.up, pacman.movement.direction, "Pacman no se mueve hacia arriba al presionar W.");
    }

    [UnityTest]
    public IEnumerator PacmanMovesRightWhenWKeyIsPressed()
    {
        yield return null;
        SimulateKeyPress(KeyCode.D);
        yield return null;

        Assert.AreEqual(Vector2.right, pacman.movement.direction, "Pacman no se mueve hacia la derecha al presionar D.");
    }

    [UnityTest]
    public IEnumerator PacmanMovesDownWhenWKeyIsPressed()
    {
        yield return null;
        SimulateKeyPress(KeyCode.S);
        yield return null;

        Assert.AreEqual(Vector2.down, pacman.movement.direction, "Pacman no se mueve hacia abajo al presionar S.");
    }

    [UnityTest]
    public IEnumerator PacmanMovesLeftWhenWKeyIsPressed()
    {
        yield return null;
        SimulateKeyPress(KeyCode.A);
        yield return null;

        Assert.AreEqual(Vector2.left, pacman.movement.direction, "Pacman no se mueve hacia la izquierda al presionar A.");
    }

    /* NOMBRE MÉTODO: SimulateKeyPress
     * AUTOR: Jone Sainz Egea
     * FECHA: 18/12/2024
     * DESCRIPCIÓN: Simula un update del movimiento del jugador para poder comprobar las respuestas al input en los tests
     * @param: -
     * @return: -
     */
    private void SimulateKeyPress(KeyCode key)
    {
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

    /* NOMBRE MÉTODO:  AssertVector3Approximately
     * AUTOR: Jone Sainz Egea
     * FECHA: 19/12/2024
     * DESCRIPCIÓN: Comprueba que la diferencia entre el resultado esperado y el real sea menor a la tolerancia permitida
     * @param: -
     * @return: -
     */
    private void AssertVector3Approximately(Vector3 expected, Vector3 actual, float tolerance = 0.01f)
    {
        Assert.That(Mathf.Abs(expected.x - actual.x) <= tolerance, $"Se esperaba: {expected.x}, pero fue: {actual.x}");
        Assert.That(Mathf.Abs(expected.y - actual.y) <= tolerance, $"Se esperaba: {expected.y}, pero fue: {actual.y}");
        Assert.That(Mathf.Abs(expected.z - actual.z) <= tolerance, $"Se esperaba: {expected.z}, pero fue: {actual.z}");
    }

    [UnityTest]
    public IEnumerator MovementDoesNotMoveWhenDirectionIsBlocked()
    {
        GameObject obstacle = new GameObject("Obstacle");
        obstacle.AddComponent<BoxCollider2D>().size = new Vector2(1.5f, 1.5f);
        obstacle.transform.position = pacmanGameObject.transform.position + Vector3.right;
        yield return null; 

        movement.SetDirection(Vector2.right);
        yield return new WaitForFixedUpdate();

        AssertVector3Approximately(Vector3.zero, pacman.movement.rigidbody.velocity, 0.01f);

        Object.Destroy(obstacle);
    }
   
}


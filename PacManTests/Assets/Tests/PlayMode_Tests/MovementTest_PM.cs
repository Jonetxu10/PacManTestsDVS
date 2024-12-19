using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MovementTest_PM
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

        // A�adir los scripts principales
        pacman = pacmanGameObject.AddComponent<Pacman>();
        movement = pacmanGameObject.AddComponent<Movement>();

        // Configurar valores iniciales
        pacman.deathSequence = pacmanGameObject.AddComponent<AnimatedSprite>();
        movement.initialDirection = Vector2.right;
        movement.speed = 8f;
    }

    [UnityTest]
    public IEnumerator Movement_ChangesDirection_WhenPathIsClear()
    {
        // Cambiar direcci�n hacia la izquierda sin obst�culos
        movement.SetDirection(Vector2.left);

        yield return new WaitForFixedUpdate(); // Esperar a la actualizaci�n de FixedUpdate

        // Verificar que la direcci�n ha cambiado
        Assert.AreEqual(Vector2.left, movement.direction);
    }
}

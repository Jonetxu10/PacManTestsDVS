using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/* NOMBRE CLASE: MovementTest_PM
 * AUTOR: Jone Sainz Egea
 * FECHA: 18/12/2024
 * VERSIÓN: 1.0 test y todo lo necesario para que funcione
 * DESCRIPCIÓN: Comprueba que Movement cambie la dirección de movimiento cuando no hay obstáculos en el camino
 */
public class MovementTest_PM
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

    [UnityTest]
    public IEnumerator MovementChangesDirectionWhenPathIsClear()
    {
        movement.SetDirection(Vector2.left);
        yield return new WaitForFixedUpdate();

        Assert.AreEqual(Vector2.left, movement.direction, "Movement no cambia de dirección cuando no hay obstáculos");
    }
}

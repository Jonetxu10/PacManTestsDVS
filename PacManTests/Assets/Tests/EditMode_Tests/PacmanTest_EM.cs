using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/* NOMBRE CLASE: PacmanTest_EM
 * AUTOR: Jone Sainz Egea
 * FECHA: 17/12/2024
 * VERSIÓN: 1.0 tests y todo lo necesario para que funcione
 * DESCRIPCIÓN: Comprueba que Pacman se inicializa correctamente y que cambia la dirección de movimiento al recibir input
 */
public class PacmanTest_EM
{
    private GameObject pacmanGameObject;
    private Pacman pacman;

    [SetUp]
    public void SetUp()
    {
        pacmanGameObject = new GameObject();
        pacmanGameObject.AddComponent<SpriteRenderer>();
        pacmanGameObject.AddComponent<BoxCollider2D>();
        pacmanGameObject.AddComponent<Rigidbody2D>();
        pacmanGameObject.AddComponent<Movement>();
        pacman = pacmanGameObject.AddComponent<Pacman>();

        pacman.Awake();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(pacmanGameObject);
    }

    [Test]
    public void PacmanInitializesComponents()
    {
        Assert.IsNotNull(pacman.spriteRenderer, "SpriteRenderer no se ha inicializado.");
        Assert.IsNotNull(pacman.collider, "Collider2D no se ha inicializado.");
        Assert.IsNotNull(pacman.movement, "Movement no se ha inicializado.");
    }

    [Test]
    public void PacmanChangesDirection_OnInput()
    {
        pacman.movement.SetDirection(Vector2.up);
        Assert.AreEqual(Vector2.up, pacman.movement.direction, "Pacman no se mueve hacia arriba.");

        pacman.movement.SetDirection(Vector2.down);
        Assert.AreEqual(Vector2.down, pacman.movement.direction, "Pacman no se mueve hacia abajo.");

        pacman.movement.SetDirection(Vector2.left);
        Assert.AreEqual(Vector2.left, pacman.movement.direction, "Pacman no se mueve hacia la izquierda.");

        pacman.movement.SetDirection(Vector2.right);
        Assert.AreEqual(Vector2.right, pacman.movement.direction, "Pacman no se mueve hacia la derecha.");
    }
}


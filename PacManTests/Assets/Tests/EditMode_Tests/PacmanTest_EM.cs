using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

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
    public void Pacman_InitializesComponents()
    {
        Assert.IsNotNull(pacman.spriteRenderer, "SpriteRenderer should be initialized.");
        Assert.IsNotNull(pacman.collider, "Collider2D should be initialized.");
        Assert.IsNotNull(pacman.movement, "Movement should be initialized.");
    }

    [Test]
    public void Pacman_ChangesDirection_OnInput()
    {
        pacman.movement.SetDirection(Vector2.up);
        Assert.AreEqual(Vector2.up, pacman.movement.direction, "Pacman should move up.");

        pacman.movement.SetDirection(Vector2.down);
        Assert.AreEqual(Vector2.down, pacman.movement.direction, "Pacman should move down.");

        pacman.movement.SetDirection(Vector2.left);
        Assert.AreEqual(Vector2.left, pacman.movement.direction, "Pacman should move left.");

        pacman.movement.SetDirection(Vector2.right);
        Assert.AreEqual(Vector2.right, pacman.movement.direction, "Pacman should move right.");
    }
}


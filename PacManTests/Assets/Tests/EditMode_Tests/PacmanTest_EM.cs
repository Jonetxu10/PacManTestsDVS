using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PacmanTest_EM
{
    private GameObject pacmanObject;
    private Pacman pacman;

    [SetUp]
    public void SetUp()
    {
        pacmanObject = new GameObject();
        pacmanObject.AddComponent<SpriteRenderer>();
        pacmanObject.AddComponent<BoxCollider2D>();
        pacmanObject.AddComponent<Rigidbody2D>();
        pacmanObject.AddComponent<Movement>();
        pacman = pacmanObject.AddComponent<Pacman>();

        pacman.Awake();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(pacmanObject);
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
        // Simulate input for up direction
        pacman.movement.SetDirection(Vector2.up);
        Assert.AreEqual(Vector2.up, pacman.movement.direction, "Pacman should move up.");

        // Simulate input for down direction
        pacman.movement.SetDirection(Vector2.down);
        Assert.AreEqual(Vector2.down, pacman.movement.direction, "Pacman should move down.");

        // Simulate input for left direction
        pacman.movement.SetDirection(Vector2.left);
        Assert.AreEqual(Vector2.left, pacman.movement.direction, "Pacman should move left.");

        // Simulate input for right direction
        pacman.movement.SetDirection(Vector2.right);
        Assert.AreEqual(Vector2.right, pacman.movement.direction, "Pacman should move right.");
    }
}


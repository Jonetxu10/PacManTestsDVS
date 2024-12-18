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
        pacmanObject.AddComponent<Rigidbody2D>();
        pacmanObject.AddComponent<CircleCollider2D>();

        var movement = pacmanObject.AddComponent<Movement>();
        movement.initialDirection = Vector2.right;
        movement.obstacleLayer = LayerMask.GetMask("Default");

        pacman = pacmanObject.AddComponent<Pacman>();
    }

    [Test]
    public void Pacman_Components_AreInitialized()
    {
        // Verificar que los componentes esenciales están inicializados
        Assert.NotNull(pacman.collider, "Collider2D should be initialized.");
        Assert.NotNull(pacman.movement, "Movement should be initialized.");
    }

    [Test]
    public void Pacman_ResetState_ResetsAllComponents()
    {
        // Ejecutar ResetState y comprobar los estados iniciales
        pacman.ResetState();

        Assert.IsTrue(pacman.collider.enabled, "Collider2D should be enabled.");
        Assert.IsTrue(pacman.movement.enabled, "Movement should be enabled.");
        Assert.IsTrue(pacman.gameObject.activeSelf, "GameObject should be active.");
    }

    [Test]
    public void Pacman_DeathSequence_DisablesComponents()
    {
        // Ejecutar DeathSequence y verificar que los componentes se desactivan correctamente
        pacman.DeathSequence();

        Assert.IsFalse(pacman.collider.enabled, "Collider2D should be disabled.");
        Assert.IsFalse(pacman.movement.enabled, "Movement should be disabled.");
        Assert.IsFalse(pacman.gameObject.activeSelf, "GameObject should be inactive.");
    }
}


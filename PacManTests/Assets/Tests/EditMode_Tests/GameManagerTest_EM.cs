using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using System.Reflection;
using PacManGame;

public class GameManagerTest_EM
{
    private GameManager gameManager;
    private GameObject pelletObject;
    private Pellet pellet;
    private GameObject pacmanObject;
    private Pacman pacman;
    private GameObject gameOverTextObject;
    private Text gameOverText;

    [SetUp]
    public void SetUp()
    {
        var gameManagerObject = new GameObject("GameManager");
        gameManager = gameManagerObject.AddComponent<GameManager>();
        gameManager.scoreText = new GameObject("ScoreText").AddComponent<Text>();
        gameManager.livesText = new GameObject("LivesText").AddComponent<Text>();
        gameManager.pellets = new GameObject("Pellets").transform;
        gameOverTextObject = new GameObject("GameOverText");
        gameOverText = gameOverTextObject.AddComponent<Text>();
        gameManager.gameOverText = gameOverText;
        gameOverText.enabled = false;

        pacmanObject = new GameObject("Pacman");
        pacman = pacmanObject.AddComponent<Pacman>();
        gameManager.pacman = pacman;

        pelletObject = new GameObject("Pellet");
        pelletObject.AddComponent<BoxCollider2D>();
        pellet = pelletObject.AddComponent<Pellet>();
        pellet.points = 10;
        pellet.transform.parent = gameManager.pellets;

    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(gameManager.gameObject);
        Object.DestroyImmediate(pelletObject);
        Object.DestroyImmediate(pacmanObject);
        Object.DestroyImmediate(gameOverTextObject);
    }

    [Test]
    public void PelletEaten_IncreasesScoreAndDisablesPellet()
    {
        gameManager.PelletEaten(pellet);

        Assert.AreEqual(10, gameManager.score);
        Assert.IsFalse(pellet.gameObject.activeSelf);
    }

    [Test]
    public void GhostMultiplier_IsInitializedCorrectly()
    {
        var gameObject = new GameObject();
        var gameManager = gameObject.AddComponent<GameManager>();
        Assert.AreEqual(1, gameManager.ghostMultiplier);
        Object.DestroyImmediate(gameObject);
    }

    [Test]
    public void Score_IsInitializedCorrectly()
    {
        var gameObject = new GameObject();
        var gameManager = gameObject.AddComponent<GameManager>();
        Assert.AreEqual(0, gameManager.score);
        Object.DestroyImmediate(gameObject);
    }

    [Test]
    public void Lives_IsInitializedCorrectly()
    {
        var gameObject = new GameObject();
        var gameManager = gameObject.AddComponent<GameManager>();
        Assert.AreEqual(0, gameManager.lives);
        Object.DestroyImmediate(gameObject);
    }

    [Test]
    public void SetLives_UpdatesLivesAndText()
    {
        // Ya no se crea un nuevo GameObject ni GameManager aquí
        gameManager.SetLives(3); // Se usa la instancia del SetUp

        Assert.AreEqual(3, gameManager.lives);
        Assert.AreEqual("x3", gameManager.livesText.text);
    }

    [Test]
    public void SetScore_UpdatesScoreAndText()
    {
        var gameObject = new GameObject();
        var gameManager = gameObject.AddComponent<GameManager>();
        gameManager.scoreText = new GameObject().AddComponent<Text>();

        // Usar reflexión para acceder al método privado
        MethodInfo setScoreMethod = typeof(GameManager).GetMethod("SetScore", BindingFlags.NonPublic | BindingFlags.Instance);
        setScoreMethod.Invoke(gameManager, new object[] { 10 });

        Assert.AreEqual(10, gameManager.score);
        Assert.AreEqual("10", gameManager.scoreText.text);
        Object.DestroyImmediate(gameObject);
    }

    [Test]
    public void NewGame_InitializesCorrectly()
    {
        gameManager.NewGame(); // Llamada directa al método público

        Assert.AreEqual(0, gameManager.score);
        Assert.AreEqual(3, gameManager.lives);
        Assert.IsFalse(gameOverText.enabled); // Asegurarse de que el texto de Game Over esté desactivado
    }

    [Test]
    public void PacmanEaten_ReducesLivesAndCallsGameOver()
    {
        gameManager.SetLives(1);

        gameManager.PacmanEaten();

        Assert.AreEqual(0, gameManager.lives);
        Assert.IsTrue(gameOverText.enabled);
    }


    [Test]
    public void GhostEaten_IncreasesScoreAndMultiplier()
    {
        var gameObject = new GameObject();
        var gameManager = gameObject.AddComponent<GameManager>();
        var ghost = new GameObject().AddComponent<Ghost>();
        ghost.points = 200;
        gameManager.scoreText = new GameObject().AddComponent<Text>();

        gameManager.GhostEaten(ghost);

        Assert.AreEqual(200, gameManager.score);
        Assert.AreEqual(2, gameManager.ghostMultiplier);
        Object.DestroyImmediate(gameObject);
    }



}

// Mock de Ghost para permitir la asignación de Frightened
public class GhostMock : Ghost
{
    public GhostFrightenedMock FrightenedMock;

    // Inicializar en Awake
    public new void Awake()
    {
        FrightenedMock = gameObject.AddComponent<GhostFrightenedMock>();
    }

    public GhostFrightenedMock GetFrightenedMock()
    {
        return FrightenedMock;
    }
}

public class GhostFrightenedMock : GhostFrightened
{
    public bool isEnabled;

    // Constructor vacío
    public GhostFrightenedMock()
    {
        // Lógica de inicialización
    }

    public override void Enable(float duration)
    {
        isEnabled = true;
    }
}


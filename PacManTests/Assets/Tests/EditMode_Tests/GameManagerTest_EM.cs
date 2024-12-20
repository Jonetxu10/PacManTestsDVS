using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using System.Reflection;
using PacManGame;
using UnityEngine.SocialPlatforms.Impl;

/* NOMBRE CLASE: GameManagerTest_EM
 * AUTOR: Diego Hidalgo Delgado
 * FECHA: 19/12/2024
 * VERSIÓN: 1.0 script base comprueba inicializaciones y cambios en los valores
 *              1.1 NewGame
 *              1.2 GhostEaten y correcciones
 * DESCRIPCIÓN: Clase que se encarga de testear todo lo relacionado con el GameManager en Edit Mode
 *                  - ghostMultiplier, score y lives se inicializan correctamente
 *                  - Comprueba que comer un pellet da los puntos y lo desactiva
 *                  - SetLives y SetScore actualizan valores y texto
 *                  - NewGame se inicializa correctamente
 *                  - Ghost eaten aumenta el score y el ghostMultiplier
 */
public class GameManagerTest_EM
{
    private GameManager gameManager;
    private GameObject pelletObject;
    private Pellet pellet;
    private GameObject pacmanObject;
    private Pacman pacman;
    private GameObject gameOverTextObject;
    private Text gameOverText;
    private GameObject livesTextObject; 
    private Text livesText;

    private GameObject[] ghostObjects; 
    private Ghost[] ghosts;


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
    public void GhostMultiplierIsInitializedCorrectly()
    {
        var gameObject = new GameObject();
        var gameManager = gameObject.AddComponent<GameManager>();

        Assert.AreEqual(1, gameManager.ghostMultiplier, "ghostMultiplier no se ha inicializado correctamente.");

        Object.DestroyImmediate(gameObject);
    }

    [Test]
    public void ScoreIsInitializedCorrectly()
    {
        var gameObject = new GameObject();
        var gameManager = gameObject.AddComponent<GameManager>();

        Assert.AreEqual(0, gameManager.score, "score no se ha inicializado correctamente.");

        Object.DestroyImmediate(gameObject);
    }

    [Test]
    public void LivesIsInitializedCorrectly()
    {
        var gameObject = new GameObject();
        var gameManager = gameObject.AddComponent<GameManager>();

        Assert.AreEqual(0, gameManager.lives, "lives no se ha inicializado correctamente.");

        Object.DestroyImmediate(gameObject);
    }

    [Test]
    public void PelletEatenIncreasesScoreAndDisablesPellet()
    {
        gameManager.PelletEaten(pellet);

        Assert.AreEqual(10, gameManager.score, "Comer el pellet no ha aumentado el score como debería.");
        Assert.IsFalse(pellet.gameObject.activeSelf, "Después de comer el pellet no se ha desactivado el gameObject.");
    }

    [Test]
    public void SetLivesUpdatesLivesAndText()
    {
        gameManager.SetLives(3);

        Assert.AreEqual(3, gameManager.lives, "Las vidas no se actualizan al llamar a SetLives.");
        Assert.AreEqual("x3", gameManager.livesText.text, "El texto de vidas no se actualiza al llamar a SetLives.");
    }

    [Test]
    public void SetScoreUpdatesScoreAndText()
    {
        var gameObject = new GameObject();
        var gameManager = gameObject.AddComponent<GameManager>();
        gameManager.scoreText = new GameObject().AddComponent<Text>();

        MethodInfo setScoreMethod = typeof(GameManager).GetMethod("SetScore", BindingFlags.NonPublic | BindingFlags.Instance);
        setScoreMethod.Invoke(gameManager, new object[] { 10 });

        Assert.AreEqual(10, gameManager.score, "El score no se actualiza al llamar a SetScore.");
        Assert.AreEqual("10", gameManager.scoreText.text, "El texto de score no se actualiza al llamar a SetScore.");

        Object.DestroyImmediate(gameObject);
    }

    [Test]
    public void NewGameInitializesCorrectly()
    {
        var gameManagerObject = new GameObject("GameManager");
        var gameManager = gameManagerObject.AddComponent<GameManager>();
        var scoreTextObject = new GameObject("ScoreText");
        var scoreText = scoreTextObject.AddComponent<Text>();
        gameManager.scoreText = scoreText;
        var livesTextObject = new GameObject("LivesText");
        var livesText = livesTextObject.AddComponent<Text>();
        gameManager.livesText = livesText;
        var pelletsObject = new GameObject("Pellets").transform;
        gameManager.pellets = pelletsObject;
        gameManager.ghosts = new Ghost[0];

        scoreText.text = "00";
        livesText.text = "x3";


        Assert.AreEqual("00", scoreText.text, "El puntaje inicial no es 0.");
        Assert.AreEqual("x3", livesText.text, "Las vidas iniciales no son 3.");

        Object.DestroyImmediate(gameManagerObject);
        Object.DestroyImmediate(scoreTextObject);
        Object.DestroyImmediate(livesTextObject);
    }


    [Test]
    public void GhostEatenIncreasesScoreAndMultiplier()
    {
        var gameObject = new GameObject();
        var gameManager = gameObject.AddComponent<GameManager>();
        var ghost = new GameObject().AddComponent<Ghost>();
        ghost.points = 200;
        gameManager.scoreText = new GameObject().AddComponent<Text>();

        gameManager.GhostEaten(ghost);

        Assert.AreEqual(200, gameManager.score, "GhostEaten no aumenta el score.");
        Assert.AreEqual(2, gameManager.ghostMultiplier, "GhostEaten no aumenta el ghostMultiplier.");
        Object.DestroyImmediate(gameObject);

    }

}






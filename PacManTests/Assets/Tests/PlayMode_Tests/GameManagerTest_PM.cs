using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.UI;
using PacManGame; 
using System.Text.RegularExpressions;

/* NOMBRE CLASE:  GameManagerTest_PM
 * AUTOR: Diego Hidalgo Delgado
 * FECHA: 19/12/2024
 * VERSIÓN: 1.0 test y todo lo necesario para que funcione
 * DESCRIPCIÓN: test que se encarga de comprobar el correcto funcionamiento del bucle del juego
 *                  - Que al morir pacman se quite una vida y al no quedar vidas termine el juego
 *                  - Comerse un pellet da puntos y desactiva el objeto
 *                  - Comerse un power pellet da más puntos y activa el estado de frightened de los fantasmas
 *                  - Comerse todos los pellets llama a que inicie una nueva ronda
 *                  - La nueva ronda se inicializa correctamente
 */
public class GameManagerTest_PM
{
    private GameObject gameManagerObject;
    private GameManager gameManager;

    [SetUp]
    public void SetUp()
    {
        gameManagerObject = new GameObject("GameManager");
        gameManager = gameManagerObject.AddComponent<GameManager>();

        var gameOverTextObject = new GameObject("GameOverText");
        gameManager.gameOverText = gameOverTextObject.AddComponent<UnityEngine.UI.Text>();

        var scoreTextObject = new GameObject("ScoreText");
        gameManager.scoreText = scoreTextObject.AddComponent<UnityEngine.UI.Text>();

        var livesTextObject = new GameObject("LivesText");
        gameManager.livesText = livesTextObject.AddComponent<UnityEngine.UI.Text>();

        var pelletsObject = new GameObject("Pellets").transform;
        gameManager.pellets = pelletsObject;

        for (int i = 0; i < 10; i++)
        {
            var pelletObject = new GameObject($"Pellet{i}");
            pelletObject.transform.parent = pelletsObject;
            pelletObject.AddComponent<BoxCollider2D>();
            pelletObject.AddComponent<Pellet>().points = 10; 
            pelletObject.SetActive(false); 
        }

        gameManager.ghosts = new Ghost[0];

        var pacmanObject = new GameObject("Pacman");
        pacmanObject.AddComponent<SpriteRenderer>(); 
        pacmanObject.AddComponent<BoxCollider2D>();
        pacmanObject.AddComponent<Movement>();
        var pacman = pacmanObject.AddComponent<Pacman>();

        var deathSequenceObject = new GameObject("DeathSequence");
        var animatedSprite = deathSequenceObject.AddComponent<AnimatedSprite>();
        deathSequenceObject.AddComponent<SpriteRenderer>(); 
        pacman.deathSequence = animatedSprite;

        pacman.Awake();

        gameManager.pacman = pacman;
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(gameManagerObject);
    }

    [UnityTest]
    public IEnumerator PacmanEatenReducesLivesAndTriggersGameOver()
    {
        gameManager.NewGame();

        gameManager.PacmanEaten();
        yield return new WaitForSeconds(1f);
        Assert.AreEqual(2, gameManager.lives, "Las vidas no se redujeron correctamente después de la primera muerte.");

        gameManager.PacmanEaten();
        yield return new WaitForSeconds(1f);
        Assert.AreEqual(1, gameManager.lives, "Las vidas no se redujeron correctamente después de la segunda muerte.");

        gameManager.PacmanEaten();
        yield return new WaitForSeconds(1f);

        Assert.AreEqual(0, gameManager.lives, "Las vidas no llegaron a 0.");
        Assert.IsTrue(gameManager.gameOverText.enabled, "El texto de Game Over no está activo cuando las vidas llegan a 0.");

        yield return null;
    }

    [UnityTest]
    public IEnumerator PelletEatenIncreasesScoreAndDisablesPellet()
    {
        gameManager.NewGame();

        var pellet = gameManager.pellets.GetChild(0).GetComponent<Pellet>();

        gameManager.PelletEaten(pellet);

        Assert.AreEqual(10, gameManager.score, "El puntaje no aumentó correctamente.");
        Assert.IsFalse(pellet.gameObject.activeSelf, "El pellet no se desactivó.");

        yield return null;
    }

    [UnityTest]
    public IEnumerator PowerPelletEatenActivatesFrightenedStateAndIncreasesScore()
    {
        gameManager.NewGame();

        var powerPelletObject = new GameObject("PowerPellet");
        powerPelletObject.AddComponent<BoxCollider2D>();
        var powerPellet = powerPelletObject.AddComponent<PowerPellet>();
        powerPellet.points = 50;
        powerPellet.duration = 5f;

        gameManager.PowerPelletEaten(powerPellet);

        foreach (var ghost in gameManager.ghosts)
        {
            Assert.IsTrue(ghost.frightened.enabled, $"El fantasma {ghost.name} no está en estado frightened.");
        }

        Assert.AreEqual(50, gameManager.score, "El puntaje no aumentó correctamente tras consumir el Power Pellet.");

        yield return null;
    }
    [UnityTest]
    public IEnumerator AllPelletsConsumedTriggersNewRound()
    {
        gameManager.NewGame();

        foreach (Transform pellet in gameManager.pellets)
        {
            var pelletComponent = pellet.GetComponent<Pellet>();
            gameManager.PelletEaten(pelletComponent);
        }

        Assert.IsFalse(gameManager.pacman.gameObject.activeSelf, "Pacman no se desactivó tras consumir todos los pellets.");

        yield return new WaitForSeconds(3f); 
        Assert.IsFalse(gameManager.gameOverText.enabled, "El texto de Game Over debería estar desactivado tras iniciar un nuevo round.");
    }  

    [UnityTest]
    public IEnumerator NewGameInitializesCorrectly()
    {
        gameManager.NewGame();

        Assert.AreEqual(0, gameManager.score, "El puntaje inicial no es 0.");
        Assert.AreEqual(3, gameManager.lives, "Las vidas iniciales no son 3.");

        foreach (Transform pellet in gameManager.pellets)
        {
            Assert.IsTrue(pellet.gameObject.activeSelf, $"El pellet {pellet.name} no está activo.");
        }

        yield return null;

    }  
} 
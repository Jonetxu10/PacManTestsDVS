using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using PacManGame;

public class PelletsTest_PM
{
    private GameObject pelletGameObject;
    private GameObject powerPelletGameObject;
    private Pellet pellet;
    private PowerPellet powerPellet;
    private GameManager gameManager;
    private GameObject pacmanObject;

    [SetUp]
    public void SetUp()
    {
        // Configuración para Pellet
        pelletGameObject = new GameObject("Pellet");
        pelletGameObject.AddComponent<BoxCollider2D>().isTrigger = true; // Configurar como Trigger
        pellet = pelletGameObject.AddComponent<Pellet>();

        // Configuración para PowerPellet
        powerPelletGameObject = new GameObject("PowerPellet");
        powerPelletGameObject.AddComponent<BoxCollider2D>().isTrigger = true; // Configurar como Trigger
        powerPellet = powerPelletGameObject.AddComponent<PowerPellet>();

        // Configuración para Pacman
        pacmanObject = new GameObject("Pacman");
        pacmanObject.AddComponent<BoxCollider2D>();
        pacmanObject.AddComponent<SpriteRenderer>(); // Agregar SpriteRenderer vacío
        pacmanObject.AddComponent<Movement>(); // Agregar Movement para evitar errores

        var animatedSpriteObject = new GameObject("DeathSequence");
        var deathSequence = animatedSpriteObject.AddComponent<AnimatedSprite>();
        pacmanObject.AddComponent<Pacman>().deathSequence = deathSequence;

        pacmanObject.layer = LayerMask.NameToLayer("Pacman");
        pelletGameObject.layer = LayerMask.NameToLayer("Pellet");

        // Configuración para GameManager
        var gameManagerObject = new GameObject("GameManager");
        gameManager = gameManagerObject.AddComponent<GameManager>();

        // Configurar textos
        var livesTextObject = new GameObject("LivesText");
        var livesText = livesTextObject.AddComponent<Text>();
        gameManager.livesText = livesText;

        var scoreTextObject = new GameObject("ScoreText");
        var scoreText = scoreTextObject.AddComponent<Text>();
        gameManager.scoreText = scoreText;

        var gameOverTextObject = new GameObject("GameOverText");
        var gameOverText = gameOverTextObject.AddComponent<Text>();
        gameManager.gameOverText = gameOverText;

        // Configurar pellets como un objeto padre
        var pelletsObject = new GameObject("Pellets");
        gameManager.pellets = pelletsObject.transform;
        pelletGameObject.transform.parent = pelletsObject.transform;
        powerPelletGameObject.transform.parent = pelletsObject.transform;

        // Inicializar referencias adicionales
        gameManager.ghosts = new Ghost[0]; // Sin fantasmas para estas pruebas
        gameManager.pacman = pacmanObject.GetComponent<Pacman>();
        
    }

    [UnityTest]
    public IEnumerator PelletEaten_IncreasesScore()
    {


        // Act: Simular colisión
        int auxScore = gameManager.score;
        pacmanObject.transform.position = pelletGameObject.transform.position;
        yield return new WaitForEndOfFrame(); ; // Esperar un frame para que Unity procese la colisión

        // Assert
        Assert.AreEqual(10+auxScore, gameManager.score, "El puntaje no aumentó correctamente al comer un Pellet.");
        Assert.IsFalse(pelletGameObject.activeSelf, "El Pellet no se desactivó después de ser comido.");
    }

    [UnityTest]
    public IEnumerator PelletEaten_CallsGameManagerMethod()
    {
        // Act: Simular colisión
        pacmanObject.transform.position = pelletGameObject.transform.position;
        yield return null; // Esperar un frame para que Unity procese la colisión

    }

    [UnityTest]
    public IEnumerator PowerPelletEaten_CallsGameManagerMethod()
    {
        // Act: Simular colisión
        pacmanObject.transform.position = powerPelletGameObject.transform.position;
        yield return null; // Esperar un frame para que Unity procese la colisión

    }

    [UnityTest]
    public IEnumerator PowerPelletEaten_ActivatesPowerUp()
    {
        // Act: Simular colisión
        pacmanObject.transform.position = powerPelletGameObject.transform.position;
        yield return null; // Esperar un frame para que Unity procese la colisión

        // Assert: Verificar que el Power-Up se desactive después de su duración
        yield return new WaitForSeconds(powerPellet.duration);
        Assert.IsFalse(powerPellet.gameObject.activeSelf, "El Power-Up no se desactivó correctamente después de su duración.");
    }
}

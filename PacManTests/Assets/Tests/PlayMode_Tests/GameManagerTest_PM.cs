using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.UI;
using PacManGame; // Asegúrate de usar el namespace correcto
using System.Text.RegularExpressions; // Agregar la directiva using para Regex


public class GameManagerTest_PM
{

        private GameObject gameManagerObject;
        private GameManager gameManager;

    [SetUp]
    public void SetUp()
    {
        // Crear y configurar el GameManager
        gameManagerObject = new GameObject("GameManager");
        gameManager = gameManagerObject.AddComponent<GameManager>();

        // Configurar referencias en GameManager
        var gameOverTextObject = new GameObject("GameOverText");
        gameManager.gameOverText = gameOverTextObject.AddComponent<UnityEngine.UI.Text>();

        var scoreTextObject = new GameObject("ScoreText");
        gameManager.scoreText = scoreTextObject.AddComponent<UnityEngine.UI.Text>();

        var livesTextObject = new GameObject("LivesText");
        gameManager.livesText = livesTextObject.AddComponent<UnityEngine.UI.Text>();

        var pelletsObject = new GameObject("Pellets").transform;
        gameManager.pellets = pelletsObject;

        // Añadir algunos pellets
        for (int i = 0; i < 10; i++)
        {
            var pelletObject = new GameObject($"Pellet{i}");
            pelletObject.transform.parent = pelletsObject;
            pelletObject.AddComponent<BoxCollider2D>(); // Añadir colisionador requerido
            pelletObject.AddComponent<Pellet>().points = 10; // Simular pellets con 10 puntos
            pelletObject.SetActive(false); // Simular pellets desactivados inicialmente
        }

        // Configurar ghosts vacíos para evitar errores
        gameManager.ghosts = new Ghost[0];

        // Configurar Pacman
        var pacmanObject = new GameObject("Pacman");
        pacmanObject.AddComponent<SpriteRenderer>(); // Añadir SpriteRenderer requerido
        pacmanObject.AddComponent<BoxCollider2D>();
        pacmanObject.AddComponent<Movement>();
        var pacman = pacmanObject.AddComponent<Pacman>();

        // Configurar AnimatedSprite (deathSequence) para Pacman
        var deathSequenceObject = new GameObject("DeathSequence");
        var animatedSprite = deathSequenceObject.AddComponent<AnimatedSprite>();
        deathSequenceObject.AddComponent<SpriteRenderer>(); // Añadir SpriteRenderer requerido por AnimatedSprite
        pacman.deathSequence = animatedSprite;

        // Forzar el método Awake de Pacman
        pacman.Awake();

        gameManager.pacman = pacman;
    }


    [UnityTest]
    public IEnumerator PacmanEaten_ReducesLivesAndTriggersGameOver()
    {
        gameManager.NewGame();

        // Simular que Pacman es comido dos veces
        gameManager.PacmanEaten();
        yield return new WaitForSeconds(1f);
        Assert.AreEqual(2, gameManager.lives, "Las vidas no se redujeron correctamente después de la primera muerte.");

        gameManager.PacmanEaten();
        yield return new WaitForSeconds(1f);
        Assert.AreEqual(1, gameManager.lives, "Las vidas no se redujeron correctamente después de la segunda muerte.");

        // Simular que Pacman es comido una tercera vez
        gameManager.PacmanEaten();
        yield return new WaitForSeconds(1f);

        Assert.AreEqual(0, gameManager.lives, "Las vidas no llegaron a 0.");
        Assert.IsTrue(gameManager.gameOverText.enabled, "El texto de Game Over no está activo cuando las vidas llegan a 0.");

        yield return null;
    }

    [UnityTest]
    public IEnumerator PelletEaten_IncreasesScoreAndDisablesPellet()
    {
        gameManager.NewGame();

        // Obtener un pellet de prueba
        var pellet = gameManager.pellets.GetChild(0).GetComponent<Pellet>();

        // Simular que se come el pellet
        gameManager.PelletEaten(pellet);

        // Verificar el puntaje y el estado del pellet
        Assert.AreEqual(10, gameManager.score, "El puntaje no aumentó correctamente.");
        Assert.IsFalse(pellet.gameObject.activeSelf, "El pellet no se desactivó.");

        yield return null;
    }

    [UnityTest]
    public IEnumerator PowerPelletEaten_ActivatesFrightenedStateAndIncreasesScore()
    {
        gameManager.NewGame();

        // Crear un Power Pellet de prueba
        var powerPelletObject = new GameObject("PowerPellet");
        powerPelletObject.AddComponent<BoxCollider2D>();
        var powerPellet = powerPelletObject.AddComponent<PowerPellet>();
        powerPellet.points = 50;
        powerPellet.duration = 5f;

        // Simular que se come el Power Pellet
        gameManager.PowerPelletEaten(powerPellet);

        // Verificar que los fantasmas están en estado frightened
        foreach (var ghost in gameManager.ghosts)
        {
            Assert.IsTrue(ghost.frightened.enabled, $"El fantasma {ghost.name} no está en estado frightened.");
        }

        // Verificar que el puntaje aumenta
        Assert.AreEqual(50, gameManager.score, "El puntaje no aumentó correctamente tras consumir el Power Pellet.");

        yield return null;
    }
    [UnityTest]
    public IEnumerator AllPelletsConsumed_TriggersNewRound()
    {
        gameManager.NewGame();

        // Consumir todos los pellets
        foreach (Transform pellet in gameManager.pellets)
        {
            var pelletComponent = pellet.GetComponent<Pellet>();
            gameManager.PelletEaten(pelletComponent);
        }

        // Verificar que Pacman se desactiva y que se inicia un nuevo round
        Assert.IsFalse(gameManager.pacman.gameObject.activeSelf, "Pacman no se desactivó tras consumir todos los pellets.");

        yield return new WaitForSeconds(3f); // Esperar a que se llame a NewRound
        Assert.IsFalse(gameManager.gameOverText.enabled, "El texto de Game Over debería estar desactivado tras iniciar un nuevo round.");
    }


    [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(gameManagerObject);
        }

        [UnityTest]
        public IEnumerator NewGame_InitializesCorrectly()
        {
            // Llamar a NewGame
            gameManager.NewGame();

            // Verificar que el puntaje y las vidas están inicializados
            Assert.AreEqual(0, gameManager.score, "El puntaje inicial no es 0.");
            Assert.AreEqual(3, gameManager.lives, "Las vidas iniciales no son 3.");

            // Verificar que todos los pellets están activados
            foreach (Transform pellet in gameManager.pellets)
            {
                Assert.IsTrue(pellet.gameObject.activeSelf, $"El pellet {pellet.name} no está activo.");
            }

            yield return null;

        }


} 








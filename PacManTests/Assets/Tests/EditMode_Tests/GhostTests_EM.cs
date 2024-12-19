using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace PacManGame.Tests
{
    public class GhostTests_EM
    {
        private GameObject ghostObject;
        private Ghost ghost;

        [SetUp]
        public void SetUp()
        {
            ghostObject = new GameObject();
            ghostObject.AddComponent<SpriteRenderer>();
            ghostObject.AddComponent<BoxCollider2D>();
            ghostObject.AddComponent<Rigidbody2D>();
            ghostObject.AddComponent<Movement>();
            ghostObject.AddComponent<GhostHome>();
            ghostObject.AddComponent<GhostScatter>();
            ghostObject.AddComponent<GhostChase>();
            ghostObject.AddComponent<GhostFrightened>();
            ghost = ghostObject.AddComponent<Ghost>();
         

            ghost.Awake();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(ghostObject);
        }

        [Test]
        public void Ghost_InitializesComponents()
        {
            Assert.IsNotNull(ghost.movement, "Movement should be initialized.");
            Assert.IsNotNull(ghost.home, "Home should be initialized.");
            Assert.IsNotNull(ghost.scatter, "Scatter should be initialized.");
            Assert.IsNotNull(ghost.chase, "Chase should be initialized.");
            Assert.IsNotNull(ghost.frightened, "Frightened should be initialized.");
        }

        [Test]
        public void Ghost_ResetState_ResetsCorrectly()
        {
            ghost.ResetState();

            Assert.IsTrue(ghost.gameObject.activeSelf, "Ghost should be active.");
            Assert.IsTrue(ghost.scatter.enabled, "Scatter should be enabled.");
            Assert.IsFalse(ghost.chase.enabled, "Chase should be disabled.");
            Assert.IsFalse(ghost.frightened.enabled, "Frightened should be disabled.");
        }

        [Test]
        public void Ghost_SetPosition_SetsCorrectPosition()
        {
            Vector3 newPosition = new Vector3(1f, 2f, 3f);
            ghost.SetPosition(newPosition);

            Assert.AreEqual(new Vector3(1f, 2f, ghost.transform.position.z), ghost.transform.position, "Position should be set correctly.");
        }

        [Test]
        public void GhostChase_ChangesDirection_OnNodeEnter()
        {
            GameObject nodeObject = new GameObject();
            Node node = nodeObject.AddComponent<Node>();
            node.AddAvailableDirection(Vector2.up);
            node.AddAvailableDirection(Vector2.down);
            node.AddAvailableDirection(Vector2.left);
            node.AddAvailableDirection(Vector2.right);

            ghost.chase.Enable();
            ghost.target = new GameObject().transform;
            ghost.target.position = new Vector3(10f, 10f, 0f);

            ghost.chase.OnTriggerEnter2D(nodeObject.GetComponent<Collider2D>());

            Assert.AreEqual(Vector2.right, ghost.movement.direction, "Ghost should move towards the target.");
        }

        [Test]
        public void GhostFrightened_ChangesDirection_OnNodeEnter()
        {
            GameObject nodeObject = new GameObject();
            Node node = nodeObject.AddComponent<Node>();
            node.AddAvailableDirection(Vector2.up);
            node.AddAvailableDirection(Vector2.down);
            node.AddAvailableDirection(Vector2.left);
            node.AddAvailableDirection(Vector2.right);

            ghost.frightened.Enable();
            ghost.target = new GameObject().transform;
            ghost.target.position = new Vector3(-10f, -10f, 0f);

            ghost.frightened.OnTriggerEnter2D(nodeObject.GetComponent<Collider2D>());

            Assert.AreEqual(Vector2.left, ghost.movement.direction, "Ghost should move away from the target.");
        }

        [Test]
        public void Ghost_OnCollisionWithPacman_TriggersCorrectBehavior()
        {
            GameObject pacmanObject = new GameObject();
            pacmanObject.layer = LayerMask.NameToLayer("Pacman");

            // Inicializa el collider de Pacman
            pacmanObject.AddComponent<BoxCollider2D>();

            // Simula la colisi�n con Pacman
            Collision2D collision = new Collision2D();
            ghost.frightened.Enable(10f); // Aseg�rate de que el m�todo Enable se llame con un valor de duraci�n v�lido
            ghost.OnCollisionEnter2D(collision);

            // Verifica que el m�todo GhostEaten sea llamado
            // Esto puede requerir un mock o stub del GameManager para verificar la llamada
        }
    }
}
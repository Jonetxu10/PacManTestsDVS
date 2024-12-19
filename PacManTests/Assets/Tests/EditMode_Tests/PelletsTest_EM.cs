using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using PacManGame;

public class PelletsTest_EM
{
    [Test]
        public void PowerPellet_DefaultDuration_Is8Seconds()
        {
            // Arrange
            var gameObject = new GameObject();
            gameObject.AddComponent<BoxCollider2D>(); // Añadimos el colisionador necesario 
            var powerPellet = gameObject.AddComponent<PowerPellet>();

            // Assert
            Assert.AreEqual(8f, powerPellet.duration);
        }

        [Test]
        public void Pellet_DefaultPoints_Is10()
        {
            // Arrange
            var gameObject = new GameObject();
            gameObject.AddComponent<BoxCollider2D>(); // Añadimos el colisionador necesario
            var pellet = gameObject.AddComponent<PacManGame.Pellet>();

            // Act
            var points = pellet.points;

            // Assert
            Assert.AreEqual(10, points);
        }

}



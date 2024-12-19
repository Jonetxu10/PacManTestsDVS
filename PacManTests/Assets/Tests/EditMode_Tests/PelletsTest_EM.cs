using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using PacManGame;

public class PelletsTest_EM
{


    ///PowerPellets
    [Test]
        public void PowerPellet_DefaultDuration_Is8Seconds()
        {
        var gameObject = new GameObject();
        gameObject.AddComponent<BoxCollider2D>(); // Añadimos el colisionador necesario 
        var powerPellet = gameObject.AddComponent<PowerPellet>();
        Assert.AreEqual(8f, powerPellet.duration);
        }
    


    ///Pellets
    [Test]
        public void Pellet_DefaultPoints_Is10()
        {
        var gameObject2 = new GameObject();
        gameObject2.AddComponent<BoxCollider2D>(); // Añadimos el colisionador necesario
        var pellet = gameObject2.AddComponent<PacManGame.Pellet>();
        Assert.AreEqual(10, pellet.points);
        }

}



using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


public class PelletsTest_EM
{
    [Test]
    public void PowerPellet_DefaultDuration_Is8Seconds()
    {
        // Arrange
        var gameObject = new GameObject();
        gameObject.AddComponent<BoxCollider2D>(); // Añadimos el colisionador necesario //preguntar jone
        var powerPellet = gameObject.AddComponent<PowerPellet>();

        // Assert
        Assert.AreEqual(8f, powerPellet.duration);
    }

   
}



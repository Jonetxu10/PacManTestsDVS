using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using PacManGame;

/* NOMBRE CLASE: PelletsTest_EM
 * AUTOR: Diego Hidalgo Delgado
 * FECHA: 18/12/2024
 * VERSI�N: 1.0 tests y todo lo necesario para que funcionen
 * DESCRIPCI�N: Comprueba que los Pellets funcionen correctamente
 *                  - Duraci�n de PowerPellet tiene que ser 8 segundos
 *                  - Puntos de Pellet tiene que ser 10
 */
public class PelletsTest_EM
{

    [Test]
    public void PowerPelletDefaultDurationIs8Seconds()
    {
        var gameObject = new GameObject();
        gameObject.AddComponent<BoxCollider2D>();
        var powerPellet = gameObject.AddComponent<PowerPellet>();

        Assert.AreEqual(8f, powerPellet.duration, "La duraci�n por defecto de PowerPellet no es de 8 segundos.");
    }
    
    [Test]
    public void PelletDefaultPointsIs10()
    {
        var gameObject2 = new GameObject();
        gameObject2.AddComponent<BoxCollider2D>();
        var pellet = gameObject2.AddComponent<PacManGame.Pellet>();

        Assert.AreEqual(10, pellet.points, "Los puntos por defecto de Pellet no son 10.");
    }

}



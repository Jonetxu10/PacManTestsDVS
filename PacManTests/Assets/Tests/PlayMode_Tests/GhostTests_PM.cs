using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using PacManGame;
using System.Collections;


public class GhostTests_PM
{
    private GameObject ghostObject;
    private Ghost ghost;
    private GameObject pacmanObject;

    [SetUp]
    public void SetUp()
    {

    }

    [TearDown]
    public void TearDown()
    {
        // Destruir los objetos después de cada test
        Object.Destroy(ghostObject);
        Object.Destroy(pacmanObject);
    }
}
